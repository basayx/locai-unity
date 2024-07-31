using System.Collections;
using System.Collections.Generic;
using System.Linq;
using LOCAI;
using TMPro;
using UnityEditor;
using UnityEngine;

public class LOCAIMenuEditor : MonoBehaviour
{
    public static List<TMP_Text> GetAllTextMeshProObjects()
    {
        var objects = FindObjectsOfType<TMP_Text>(false).ToList();
        return objects;
    }

    [MenuItem("LOCAI/One Click/TextMeshPro/Constant/All")]
    private static void OnClick_TextMeshPro_Constant_All()
    {
        var objects = GetAllTextMeshProObjects();
        foreach (var obj in objects)
        {
            if (obj.gameObject && !obj.gameObject.GetComponent<LOCAISceneObject>())
            {
                obj.gameObject.AddComponent<LOCAIConstantTMPro>();
                var go = obj.gameObject;
                LOCAIManager.SetDirty(go);
            }
        }
    }
    [MenuItem("LOCAI/One Click/TextMeshPro/Constant/Active Ones")]
    private static void OnClick_TextMeshPro_Constant_ActiveOnes()
    {
        var objects = GetAllTextMeshProObjects();
        foreach (var obj in objects)
        {
            if (obj.gameObject && !obj.gameObject.GetComponent<LOCAISceneObject>())
            {
                obj.gameObject.AddComponent<LOCAIConstantTMPro>();
                var go = obj.gameObject;
                LOCAIManager.SetDirty(go);
            }
        }
    }
    
    [MenuItem("LOCAI/One Click/TextMeshPro/Runtime/All")]
    private static void OnClick_TextMeshPro_Runtime_All()
    {
        var objects = GetAllTextMeshProObjects();
        foreach (var obj in objects)
        {
            if (obj.gameObject && !obj.gameObject.GetComponent<LOCAISceneObject>())
            {
                obj.gameObject.AddComponent<LOCAIRuntimeTMPro>();
                var go = obj.gameObject;
                LOCAIManager.SetDirty(go);
            }
        }
    }
    [MenuItem("LOCAI/One Click/TextMeshPro/Runtime/Active Ones")]
    private static void OnClick_TextMeshPro_Runtime_ActiveOnes()
    {
        var objects = GetAllTextMeshProObjects();
        foreach (var obj in objects)
        {
            if (obj.gameObject && !obj.gameObject.GetComponent<LOCAISceneObject>())
            {
                obj.gameObject.AddComponent<LOCAIRuntimeTMPro>();
                var go = obj.gameObject;
                LOCAIManager.SetDirty(go);
            }
        }
    }
    
    [MenuItem("LOCAI/One Click/TextMeshPro/Runtime Unstable/All")]
    private static void OnClick_TextMeshPro_RuntimeUnstable_All()
    {
        var objects = GetAllTextMeshProObjects();
        foreach (var obj in objects)
        {
            if (obj.gameObject && !obj.gameObject.GetComponent<LOCAISceneObject>())
            {
                obj.gameObject.AddComponent<LOCAIUnstableRuntimeTMPro>();
                var go = obj.gameObject;
                LOCAIManager.SetDirty(go);
            }
        }
    }
    [MenuItem("LOCAI/One Click/TextMeshPro/Runtime Unstable/Active Ones")]
    private static void OnClick_TextMeshPro_RuntimeUnstable_ActiveOnes()
    {
        var objects = GetAllTextMeshProObjects();
        foreach (var obj in objects)
        {
            if (obj.gameObject && !obj.gameObject.GetComponent<LOCAISceneObject>())
            {
                obj.gameObject.AddComponent<LOCAIUnstableRuntimeTMPro>();
                var go = obj.gameObject;
                LOCAIManager.SetDirty(go);
            }
        }
    }
    
    [MenuItem("LOCAI/Remove/TextMeshPro/Remove All")]
    private static void Remove_TextMeshPro_Remove()
    {
        var objects = FindObjectsOfType<LOCAITMProObject>(true);
        foreach (var obj in objects)
        {
            if (obj.gameObject)
            {
                var go = obj.gameObject;
                var sceneObject = obj.gameObject.GetComponent<LOCAISceneObject>();
                if (sceneObject)
                {
                    sceneObject.RemoveTheComponent();
                    DestroyImmediate(sceneObject);
                    DestroyImmediate(obj);
                    
                    LOCAIManager.SetDirty(go);
                }
            }
        }

        RemoveTheOthers(true);
    }
    [MenuItem("LOCAI/Remove/TextMeshPro/Remove Active Ones")]
    private static void Remove_TextMeshPro_RemoveActiveOnes()
    {
        var objects = FindObjectsOfType<LOCAITMProObject>(false);
        foreach (var obj in objects)
        {
            if (obj.gameObject)
            {
                var go = obj.gameObject;
                var sceneObject = obj.gameObject.GetComponent<LOCAISceneObject>();
                if (sceneObject)
                {
                    sceneObject.RemoveTheComponent();
                    DestroyImmediate(sceneObject);
                    DestroyImmediate(obj);
                    
                    LOCAIManager.SetDirty(go);
                }
            }
        }

        RemoveTheOthers(false);
    }

    private static void RemoveTheOthers(bool includeInactive)
    {
        var objects = FindObjectsOfType<LOCAIAdditionalPromptObject>(includeInactive);
        foreach (var obj in objects)
        {
            if (obj.gameObject)
            {
                var go = obj.gameObject;
                DestroyImmediate(obj);
                
                LOCAIManager.SetDirty(go);
            }
        }
        
        var runtimeManagers = FindObjectsOfType<LOCAIRuntimeManager>(includeInactive);
        foreach (var obj in runtimeManagers)
        {
            if (obj.gameObject)
            {
                var go = obj.gameObject;
                DestroyImmediate(go);
            }
        }
    }
    
    [MenuItem("LOCAI/Reset/Reset All")]
    private static void Reset_ResetAll()
    {
        Remove_TextMeshPro_Remove();

        LOCAIDataHolder.Instance.ResetAll();
        LOCAIManager.SetDirty(LOCAIDataHolder.Instance);
    }
    
    [MenuItem("LOCAI/Settings")]
    private static void OnClick_Select_Settings()
    {
        string path = "Assets/LOCAI/Resources/LOCAI Settings.asset";
        Selection.activeObject = AssetDatabase.LoadMainAssetAtPath(path);
    }
}
