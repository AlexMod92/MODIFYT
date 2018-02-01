using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

[assembly: System.Reflection.AssemblyVersion("1.0.*")]
public class BuildVersionHandler : MonoBehaviour
{
    public Text buildVersionText = null;

    private string buildVersionComplete = "";
    private string assemblyBuildVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
    private string unityBuildVersion = "";

    [Tooltip("p - Prototype, a - Alpha, b - Beta")]
    public string productBuilt = "p";

    private void Awake()
    {
        GenerateBuildVersion();
    }

    private void GenerateBuildVersion()
    {
        unityBuildVersion = "u" + Application.unityVersion;

        buildVersionComplete = assemblyBuildVersion + "_" + unityBuildVersion;

        buildVersionText.text = "Version: " + productBuilt + buildVersionComplete;
    }
}