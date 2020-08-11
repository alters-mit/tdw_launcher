using System.IO;
using UnityEngine;
using System.Diagnostics;
using System;


/// <summary>
/// This object can launch a controller and monitor whether they are running.
/// </summary>
public class Launcher
{

    #region FIELDS

    #region PUBLIC

    /// <summary>
    /// Returns true if the controller exists at the expected path.
    /// </summary>
    public static bool ControllerExists
    {
        get
        {
            return new FileInfo(ControllerPath).Exists;
        }
    }
    /// <summary>
    /// Returns true if the config file exists.
    /// </summary>
    public static bool ConfigExists
    {
        get
        {
            return new FileInfo(ConfigPath).Exists;
        }
    }
    /// <summary>
    /// The path to the shortcut for the controller executable.
    /// (The shortcut might include arguments.)
    /// </summary>
    public static string ControllerPath
    {
        get
        {
            string p = Path.Combine(RootDir, "tdw_controller");
            if (SystemInfo.operatingSystem.Contains("Windows"))
            {
                p += ".exe";
            }
            else if (SystemInfo.operatingSystem.Contains("OS X"))
            {
                p += ".app";
            }
            return p;
        }
    }
    /// <summary>
    /// The path to the config file.
    /// </summary>
    public static string ConfigPath
    {
        get
        {
            return Path.Combine(RootDir, "freeze.ini");
        }
    }

    #endregion

    #region PRIVATE

    /// <summary>
    /// The root directory.
    /// </summary>
    private readonly static string RootDir =
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "tdw_build/tdw_controller");
    /// <summary>
    /// The controller.py process.
    /// </summary>
    private Process controller;

    #endregion

    #endregion

    #region CONSTRUCTORS

    /// <param name="controllerArgs">Additional arguments to pass to the controller.</param>
    public Launcher(string controllerArgs)
    {
        // Initialize the controller process.
        controller = new Process();
        controller.StartInfo.FileName = ControllerPath;
        controller.StartInfo.WorkingDirectory = RootDir;

        // Parse the arguments.
        string config = File.ReadAllText(ConfigPath);
        string arguments = "";
        foreach (string line in config.Split('\n'))
        {
            if (line.StartsWith("args="))
            {
                arguments = line.Split('=')[1].Trim();
            }
        }
        // Add additional arguments.
        arguments += " " + controllerArgs;
        controller.StartInfo.Arguments = arguments;
        controller.StartInfo.UseShellExecute = false;
        controller.StartInfo.RedirectStandardOutput = true;
    }

    #endregion

    #region METHODS

    /// <summary>
    /// Launch the controller and the build.
    /// </summary>
    public void Launch()
    {
        controller.Start();
    }


    /// <summary>
    /// Returns true if the controller is still running.
    /// </summary>
    public bool IsRunning()
    {
        return !controller.HasExited;
    }


    /// <summary>
    /// Kill the controller, the build, and this application too.
    /// </summary>
    public void Kill()
    {
        if (!controller.HasExited)
        {
            controller.Kill();
        }
    }

    #endregion

}