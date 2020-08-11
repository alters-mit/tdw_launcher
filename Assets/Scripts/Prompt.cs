using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;


/// <summary>
/// Prompt the user to enter a number. Launch the controller and the build.
/// </summary>
public class Prompt : MonoBehaviour
{
    /// <summary>
    /// The input prompt.
    /// </summary>
    public InputField prompt;
    /// <summary>
    /// Press this to launch everything.
    /// </summary>
    public Button buttonOK;
    /// <summary>
    /// Any messages or warnings.
    /// </summary>
    public Text message;
    /// <summary>
    /// The launcher. If not null, something is already running.
    /// </summary>
    private Launcher launcher = null;


    private void Awake()
    {
        buttonOK.onClick.AddListener(Launch);
    }


    private void Update()
    {
        // New launcher.
        if (launcher == null)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                Launch();
            }
        }
        // Listen for completion.
        else
        {
            // Stop everything.
            if (!launcher.IsRunning())
            {
                Application.Quit();
            }
        }
    }


    /// <summary>
    /// Launch a controller and a build.
    /// </summary>
    private void Launch()
    {
        if (launcher != null)
        {
            return;
        }
        List<string> warnings = new List<string>();
        if (!Launcher.ControllerExists)
        {
            warnings.Add("No controller found at: " + Launcher.ControllerPath);
        }
        if (!Launcher.ConfigExists)
        {
            warnings.Add("No config file found at: " + Launcher.ConfigPath);
        }
        if (prompt.text == "")
        {
            warnings.Add("Please enter a number.");
        }
        if (warnings.Count > 0)
        {
            string w = "";
            for (int i = 0; i < warnings.Count; i++)
            {
                w += warnings[i];
                if (i < warnings.Count - 1)
                {
                    w += "\n";
                }
            }
            message.text = w;
        }
        else
        {
            launcher = new Launcher(prompt.text);
            launcher.Launch();
        }
    }


    private void OnApplicationQuit()
    {
        if (launcher != null)
        {
            launcher.Kill();
        }
    }
}