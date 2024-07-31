using System.Collections;
using System.Collections.Generic;
using LOCAI;
using UnityEngine;

namespace LOCAI
{
    [RequireComponent(typeof(LOCAITMProObject))]
    [AddComponentMenu("LOCAI/Other/LOCAI TMPro Twin Listener")]
    public class LOCAITMProObjectTwinListener : MonoBehaviour
    {
        private LOCAITMProObject _tmpProObject;
        private LOCAISceneObject _locaiSceneObject;
        
        void Start()
        {
            _tmpProObject = GetComponent<LOCAITMProObject>();
            
            _locaiSceneObject = GetComponent<LOCAISceneObject>();
            if (!_locaiSceneObject) _locaiSceneObject = GetComponentInParent<LOCAISceneObject>(true);
            _locaiSceneObject.OnDefineLocalizationsProcessFinished += Twin;
        }

        private void Twin(SupportedLanguages localizedLanguage, string localizedString)
        {
            _tmpProObject.TMPText.text = localizedString;
        }
    }
}
