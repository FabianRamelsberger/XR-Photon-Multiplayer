using System;
using UnityEngine;

namespace FREngine.Tools.BuildVersion.Editor
{
    using UnityEditor;
    using UnityEditor.Build;
    using UnityEditor.Build.Reporting;
    
    public class BuildVersionProcessor : IPreprocessBuildWithReport
    {
        public int callbackOrder => 0;
        private const string initialVersion = "0.0";
        
        public void OnPreprocessBuild(BuildReport report)
        {
            string currentVersion = FindCurrentVersion();
            UpdateVersion(currentVersion);
        }

        private string FindCurrentVersion()
        {
            // Split to find string of current version;
            string[] currentVersion = PlayerSettings.bundleVersion.Split('[', ']');
            
            // I not the proper format, start with the initial
            return currentVersion.Length == 1 ? initialVersion : currentVersion[1];
        }

        private void UpdateVersion(string version)
        {
            //parse out version number from string split
            if (float.TryParse(version, out float versionNumber))
            {
                // Setup new values
                float newVersion = versionNumber + 0.01f;
                string date = DateTime.Now.ToString("d");
                
                // Create new string and set in PlayerSettings
                PlayerSettings.bundleVersion = string.Format("Version [{0}] - {1}", newVersion, date);
                Debug.Log(PlayerSettings.bundleVersion);
            }
            
        }
    }
}