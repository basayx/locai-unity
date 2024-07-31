using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LOCAI
{
    public abstract class LOCAIUnstableRuntime : LOCAIRuntime
    {
        public abstract bool IsCurrentlyStable();
        
        protected override void SetStoringLanguagesList()
        {
            if (Application.isPlaying)
            {
                if (!IsStoringLanguagesCorrect)
                {
                    _originalString = string.Empty;
                    _storingLanguages = new List<SupportedLanguages>() { LOCAISettings.Instance.TargetLanguage };
                }
            }
        }
    }
}
