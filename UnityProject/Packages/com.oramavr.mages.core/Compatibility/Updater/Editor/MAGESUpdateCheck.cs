namespace MAGES.Updater.Editor
{
    using UnityEditor;
    using UnityEditor.PackageManager;
    using UnityEditor.PackageManager.Requests;
    using UnityEngine;

    /// <summary>
    /// Class responsible for checking for new MAGES version.
    /// </summary>
    [InitializeOnLoad]
    internal class MAGESUpdateCheck : MonoBehaviour
    {
        private const string KUpdateShown = "MAGESUpdateShown";

        private static ListRequest requestPackages;

        static MAGESUpdateCheck()
        {
            if (!SessionState.GetBool(KUpdateShown, false))
            {
                EditorApplication.update += CheckMAGESVersion;
            }
        }

        private static void CheckMAGESVersion()
        {
            requestPackages = Client.List();
            EditorApplication.update += Progress;
            EditorApplication.update -= CheckMAGESVersion;
        }

        private static void Progress()
        {
            if (requestPackages.IsCompleted && requestPackages.Status == StatusCode.Success)
            {
                foreach (var package in requestPackages.Result)
                {
                    if (package.name.Equals("com.oramavr.mages.core"))
                    {
                        if (!package.version.Equals(package.versions.latest) && !PlayerPrefs.HasKey("MAGESSkipVersion" + package.versions.latest))
                        {
                            if (!string.IsNullOrEmpty(package.versions.latest))
                            {
                                int answer = EditorUtility.DisplayDialogComplex("MAGES SDK Update Available", "Version " + package.versions.latest + " of MAGES SDK is available. Currently installed version is " + package.version + " .", "Install update", "Remind me later", "Skip this version");
                                if (answer == 0)
                                {
                                    Client.Add("com.oramavr.mages.core");
                                    AssetDatabase.Refresh();
                                }
                                else
                                {
                                    if (answer == 2)
                                    {
                                        PlayerPrefs.SetInt("MAGESSkipVersion" + package.versions.latest, 1);
                                    }
                                }
                            }
                        }
                    }
                }

                EditorApplication.update -= Progress;
            }
            else
            {
                if (requestPackages.Status == StatusCode.Failure)
                {
                    Debug.LogError("Error getting packages installed on editor");
                    EditorApplication.update -= Progress;
                }
            }

            SessionState.SetBool(KUpdateShown, true);
        }
    }
}