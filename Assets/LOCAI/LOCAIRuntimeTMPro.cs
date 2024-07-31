using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace LOCAI
{
    [RequireComponent(typeof(LOCAITMProObject))]
    [AddComponentMenu("LOCAI/Runtime/LOCAI TMPro (Runtime)")]
    public class LOCAIRuntimeTMPro : LOCAIRuntime
    {
        protected override string GetOriginalStringInitially() { return TMPObject.TMPText.text; }
        
        private LOCAITMProObject _tmpObject;
        public LOCAITMProObject TMPObject
        {
            get
            {
                if (!_tmpObject) _tmpObject = GetComponent<LOCAITMProObject>();
                return _tmpObject;
            }
        }
        
        protected override void ManualStart()
        {
            base.ManualStart();
            
            TMPObject.InitializeFont();
            if (!GetData().LocalizedLanguages.Contains(LOCAISettings.Instance.TargetLanguage))
            {
                DefineLocalizations();            
            }
            else
                TMPObject.TMPText.text = GetTargetLocalizedString();
        }

        protected override void DefineLocalizationsProcessFinished(SupportedLanguages lastLocalizedLanguage, string lastLocalizedString)
        {
            base.DefineLocalizationsProcessFinished(lastLocalizedLanguage, lastLocalizedString);
            TMPObject.TMPText.text = lastLocalizedString;
        }
    }
}
