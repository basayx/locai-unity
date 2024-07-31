using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace LOCAI
{
    [CustomEditor(typeof(LOCAIConfigurations))]
    public class LOCAIConfigurationsEditor : Editor
    {
        SerializedProperty _useGoogleCloudFunctionURL;
        SerializedProperty _googleCloudFunctionURL;
        
        SerializedProperty _useFirebaseFunctionsBoolean;
        SerializedProperty _firebaseFunctionName;

        SerializedProperty _useKeyVaultLogic;
        
        private void OnEnable () 
        {
            _useGoogleCloudFunctionURL = serializedObject.FindProperty(nameof(LOCAIConfigurations.UseGoogleCloudFunctionURL));
            _googleCloudFunctionURL = serializedObject.FindProperty(nameof(LOCAIConfigurations.GoogleCloudFunctionURL));
            
            _useFirebaseFunctionsBoolean = serializedObject.FindProperty(nameof(LOCAIConfigurations.UseFirebaseFunctions));
            _firebaseFunctionName = serializedObject.FindProperty(nameof(LOCAIConfigurations.FirebaseFunctionName));
            
            _useKeyVaultLogic = serializedObject.FindProperty(nameof(LOCAIConfigurations.UseKeyVaultLogic));
        }

        public override void OnInspectorGUI () 
        {
            serializedObject.Update();

            EditorGUILayout.LabelField("Predefined Deploy Options:", EditorStyles.boldLabel);
            
            if (_useGoogleCloudFunctionURL.boolValue)
            {
                EditorGUILayout.PropertyField(_useGoogleCloudFunctionURL);
                EditorGUILayout.PropertyField(_googleCloudFunctionURL);
                EditorGUILayout.HelpBox("LOCAI Manager sends the request to target Google Cloud Function via provided URL." +
                                        "\nYou can find example cloud function on LOCAI Github page.", MessageType.Info, true);
            }
            else if (_useFirebaseFunctionsBoolean.boolValue)
            {
                EditorGUILayout.HelpBox("You have to add \"LOCAI_FIREBASE\" key word to your project's Script Define Symbols", MessageType.Warning, true);
                EditorGUILayout.PropertyField(_useFirebaseFunctionsBoolean);
                EditorGUILayout.PropertyField(_firebaseFunctionName);
                EditorGUILayout.HelpBox("LOCAI Manager sends the requests to target Firebase Function." +
                                        "\nYou can find example cloud function on LOCAI Github page.", MessageType.Info, true);
            }
            else if (_useKeyVaultLogic.boolValue)
            {
                EditorGUILayout.PropertyField(_useKeyVaultLogic);
                EditorGUILayout.HelpBox("LOCAI Manager sends the requests to directly Gemini API but try to get the API key from LOCAIKeyVaultGateway script." +
                                        "\nYou can find more information about key vault on LOCAI Github page.", MessageType.Info, true);
            }
            else
            {
                EditorGUILayout.PropertyField(_useGoogleCloudFunctionURL);
                EditorGUILayout.PropertyField(_useFirebaseFunctionsBoolean);
                EditorGUILayout.PropertyField(_useKeyVaultLogic);
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
