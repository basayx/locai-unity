using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace LOCAI
{
    [CustomEditor(typeof(LOCAISettings))]
    public class LOCAISettingsEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            EditorGUILayout.HelpBox("Do not deploy your API key!\nCheck GitHub page for more info...", MessageType.Warning, true);

            DrawDefaultInspector();
            EditorGUILayout.HelpBox("Also you can use TMPro dynamic fallbacks to use single font asset to all texts.", MessageType.Info, true);
        }
    }
}
