using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace Plugins.Tools.Editor
{
    public class AndroidKeystoreAuthenticator : IPreprocessBuildWithReport
    {
        public int callbackOrder => 0;

        private const string KeystoreNameKey = "And_KeyStore_Name";
        private const string KeystorePassKey = "And_KeyStore_Pass";
        private const string KeystoreAliaNameKey = "And_KeyStore_Alias_Name";
        private const string KeystoreAliasPassKey = "And_KeyStore_Alias_Pass";
        
        public void OnPreprocessBuild(BuildReport report)
        {
            if (string.IsNullOrEmpty(PlayerSettings.Android.keystoreName))
            {
                if (!EditorPrefs.HasKey(KeystoreNameKey))
                {
                    Debug.LogWarning("Keystore is Missing File");
                    return;   
                }

                PlayerSettings.Android.keystoreName = EditorPrefs.GetString(KeystoreNameKey);
            }
            else if(!string.IsNullOrEmpty(PlayerSettings.Android.keystoreName))
            {
                EditorPrefs.SetString(KeystoreNameKey, PlayerSettings.Android.keystoreName);
            }

            if (string.IsNullOrEmpty(PlayerSettings.Android.keystorePass) && EditorPrefs.HasKey(KeystorePassKey))
            {
                PlayerSettings.Android.keystorePass = EditorPrefs.GetString(KeystorePassKey);
            }
            else if (!string.IsNullOrEmpty(PlayerSettings.Android.keystorePass))
            {
                EditorPrefs.SetString(KeystorePassKey, PlayerSettings.Android.keystorePass);
            }

            if (string.IsNullOrEmpty(PlayerSettings.Android.keyaliasName) && EditorPrefs.HasKey(KeystoreAliaNameKey))
            {
                PlayerSettings.Android.keyaliasName = EditorPrefs.GetString(KeystoreAliaNameKey);
            }
            else if (!string.IsNullOrEmpty(PlayerSettings.Android.keyaliasName))
            {
                EditorPrefs.SetString(KeystoreAliaNameKey, PlayerSettings.Android.keyaliasName);
            }

            if (string.IsNullOrEmpty(PlayerSettings.Android.keyaliasPass) && EditorPrefs.HasKey(KeystoreAliasPassKey))
            {
                PlayerSettings.Android.keyaliasPass = EditorPrefs.GetString(KeystoreAliasPassKey);
            }
            else if (!string.IsNullOrEmpty(PlayerSettings.Android.keyaliasPass))
            {
                EditorPrefs.SetString(KeystoreAliasPassKey, PlayerSettings.Android.keyaliasPass);
            }
        }
    }
}