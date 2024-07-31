using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace LOCAI
{
    [RequireComponent(typeof(LOCAITMProObject))]
    [AddComponentMenu("LOCAI/Runtime/LOCAI TMPro (Unstable Runtime)")]
    public class LOCAIUnstableRuntimeTMPro : LOCAIUnstableRuntime
    {
        protected override string GetOriginalStringInitially()
        {
            return (_lastLocalizedString != TMPObject.TMPText.text) ? TMPObject.TMPText.text : GetData().OriginalString;
        }

        private LOCAITMProObject _tmpObject;

        public LOCAITMProObject TMPObject
        {
            get
            {
                if (!_tmpObject) _tmpObject = GetComponent<LOCAITMProObject>();
                return _tmpObject;
            }
        }

        public override bool IsCurrentlyStable()
        {
            return TMPObject.TMPText.text == _lastLocalizedString && _lastLocalizedLanguage == LOCAISettings.Instance.TargetLanguage && !_isInLocalizationProcess;
        }

        protected override void ManualStart()
        {
            base.ManualStart();
            
            TMPObject.InitializeFont();
        }

        private void Update()
        {
            if (TMPObject.TMPText.text != _lastLocalizedString)
            {
                var data = GetData();
                data.OriginalString = TMPObject.TMPText.text;
                data.LocalizedStrings.Clear();
                data.LocalizedStrings.Add(data.OriginalString);

                _originalString = data.OriginalString;
                _lastLocalizedString = data.OriginalString;
                DefineLocalizations();
            }
        }

        private SupportedLanguages _lastLocalizedLanguage;
        string _lastLocalizedString = ""; 
        protected override void DefineLocalizationsProcessFinished(SupportedLanguages lastLocalizedLanguage, string lastLocalizedString)
        {
            base.DefineLocalizationsProcessFinished(lastLocalizedLanguage, lastLocalizedString);
            TMPObject.TMPText.text = lastLocalizedString;

            _lastLocalizedLanguage = lastLocalizedLanguage;
            _lastLocalizedString = lastLocalizedString;

            var data = GetData();
            string defaultLocalizedString = data.LocalizedStrings[0]; 
            data.LocalizedStrings.Clear();
            data.LocalizedStrings.Add(defaultLocalizedString);
            SupportedLanguages defaultSupportedLanguages = data.LocalizedLanguages[0];
            data.LocalizedLanguages.Clear();
            data.LocalizedLanguages.Add(defaultSupportedLanguages);
        }
    }
}