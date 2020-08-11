using System.IO;
using UnityEngine;
using System.Diagnostics;
using System;


/// <summary>
/// This object can launch a controller and a build, and monitor whether they are running.
/// </summary>
public class Launcher
{
    /// <summary>
    /// The root directory of the build and the controller.
    /// </summary>
    private readonly static string RootDir =
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "tdw_build");
    /// <summary>
    /// The directory containing the controller executable.
    /// </summary>
    private readonly static string ControllerDir = Path.Combine(RootDir, "tdw_controller");
    /// <summary>
    /// The controller.py process.
    /// </summary>
    private Process controller;
    /// <summary>
    /// The TDW.exe process.
    /// </summary>
    private Process build;


    /// <summary>
    /// The path to the shortcut for the controller executable.
    /// (The shortcut might include arguments.)
    /// </summary>
    private string ControllerPath
    {
        get
        {
            string p = Path.Combine(ControllerDir, "tdw_controller");
            if (SystemInfo.operatingSystem.Contains("Windows"))
            {
                p += ".lnk";
            }
            else if (SystemInfo.operatingSystem.Contains("OS X"))
            {
                p += ".app";
            }
            else if (SystemInfo.operatingSystem.Contains("Linux"))
            {
                p += ".sh";
            }
            return p;
        }
    }
    /// <summary>
    /// Returns the path to the build.
    /// </summary>
    private string BuildPath
    {
        get
        {
            string p = Path.Combine(RootDir, "TDW/TDW");
            if (SystemInfo.operatingSystem.Contains("Windows"))
            {
                p += ".exe";
            }
            else if (SystemInfo.operatingSystem.Contains("OS X"))
            {
                p += ".app";
            }
            else if (SystemInfo.operatingSystem.Contains("Linux"))
            {
                p += ".x86_64";
            }
            return p;
        }
    }


    /// <param name="controllerArgs">Additional arguments to pass to the controller.</param>
    public Launcher(string controllerArgs)
    {
        // Initialize the controller process.
        controller = new Process();
        controller.StartInfo.FileName = ControllerPath;
        controller.StartInfo.WorkingDirectory = ControllerDir;
        controller.StartInfo.Arguments = controllerArgs;
        controller.StartInfo.UseShellExecute = false;
        controller.StartInfo.RedirectStandardOutput = true;

        // Initialize the build process.
        build = new Process();
        build.StartInfo.FileName = BuildPath;
        build.StartInfo.Arguments = "-screenWidth=1024 -screenHeight=1024";
    }


    /// <summary>
    /// Launch the controller and the build.
    /// </summary>
    public void Launch()
    {
        controller.Start();
        build.Start();
    }


    /// <summary>
    /// Returns true if the controller is still running.
    /// </summary>
    public bool IsRunning()
    {
        return !controller.HasExited;
    }


    /// <summary>
    /// Kill the controller and build processes.
    /// </summary>
    public void Kill()
    {
        if (!controller.HasExited)
        {
            controller.Kill();
        }
        if (!build.HasExited)
        {
            build.Kill();
        }
    }
}