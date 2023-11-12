using System.Diagnostics;
using UnityEditor;

public class MenuItems
{
    [MenuItem("Tools/Start Testing game (1 instance)")]
    private static void OneInstance()
    {
        Process proc = new Process();
        proc.StartInfo.FileName = "C:\\Github\\UnityNGODemo\\Builds\\UnityNGODemo.exe";
        proc.Start();
    }

    [MenuItem("Tools/Start Testing game (2 instance)")]
    private static void TwoInstances()
    {
        Process proc = new Process();
        proc.StartInfo.FileName = "C:\\Github\\UnityNGODemo\\Builds\\UnityNGODemo.exe";
        proc.Start();
        proc.Start();
    }
}