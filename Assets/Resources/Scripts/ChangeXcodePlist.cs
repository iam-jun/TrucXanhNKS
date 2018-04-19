#if UNITY_5 && UNITY_IOS
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using System.Collections;
using UnityEditor.iOS.Xcode;
using System.IO;

public class ChangeXcodePlist
{
    [PostProcessBuild]
    public static void ChangeWifiPlist(BuildTarget buildTarget, string pathToBuiltProject)
    {
        if (buildTarget == BuildTarget.iOS)
        {
            // Get plist
            string plistPath = pathToBuiltProject + "/Info.plist";
            PlistDocument plist = new PlistDocument();
            plist.ReadFromString(File.ReadAllText(plistPath));

            // Get root
            PlistElementDict rootDict = plist.root;

            // background location useage key (new in iOS 8)
            rootDict.SetBoolean("UIRequiresPersistentWiFi", true);

            // background modes
            PlistElementArray bgModes = rootDict.CreateArray("UIBackgroundModes");
            bgModes.AddString("location");
            bgModes.AddString("fetch");

            // Write to file
            File.WriteAllText(plistPath, plist.WriteToString());
        }
    }
}
#endif