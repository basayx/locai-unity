using System;
using UnityEngine;
using UnityEditor;

namespace LOCAI
{
    [CustomEditor(typeof(LOCAIConstantTMPro))]
    public class Editor_LOCAIConstantTMPro : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            LOCAIConstantTMPro myScript = (LOCAIConstantTMPro)target;
            if(GUILayout.Button("Remove The Component"))
            {
                var tmpObject = myScript.GetComponent<LOCAITMProObject>();
                myScript.RemoveTheComponent();
                DestroyImmediate(tmpObject);
            }
        }
    }
    
    [CustomEditor(typeof(LOCAIRuntimeTMPro))]
    public class Editor_LOCAIRuntimeTMPro : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            LOCAIRuntimeTMPro myScript = (LOCAIRuntimeTMPro)target;
            if(GUILayout.Button("Remove The Component"))
            {
                var tmpObject = myScript.GetComponent<LOCAITMProObject>();
                myScript.RemoveTheComponent();
                DestroyImmediate(tmpObject);
            }
        }
    }
    
    [CustomEditor(typeof(LOCAIUnstableRuntimeTMPro))]
    public class Editor_LOCAIAutoRuntimeTMPro : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            LOCAIUnstableRuntimeTMPro myScript = (LOCAIUnstableRuntimeTMPro)target;
            if(GUILayout.Button("Remove The Component"))
            {
                var tmpObject = myScript.GetComponent<LOCAITMProObject>();
                myScript.RemoveTheComponent();
                DestroyImmediate(tmpObject);
            }
        }
    }
}
