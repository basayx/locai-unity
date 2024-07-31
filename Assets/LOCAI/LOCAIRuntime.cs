using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LOCAI
{
    public abstract class LOCAIRuntime : LOCAISceneObject
    {
        protected override bool IsStoringLanguagesCorrect => _storingLanguages.Count == 1 && _storingLanguages[0] == LOCAISettings.Instance.TargetLanguage;
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
        //
        // protected override string GetOriginalStringInitially()
        // {
        //     return RuntimeLocalizedString;
        // }
        //
        // protected string _runtimeLocalizedString;
        // protected string RuntimeLocalizedString
        // {
        //     get
        //     {
        //         if (string.IsNullOrEmpty(_runtimeLocalizedString)) _runtimeLocalizedString = GetRuntimeString();
        //         return _runtimeLocalizedString;
        //     }
        // }
        // protected virtual string GetRuntimeString() { return OriginalString; }
        //
        // protected void ApplyRuntimeLocalization()
        // {
        //     if (_dataIndex >= 0)
        //     {
        //         DefineLocalizations(false);
        //     }
        // }
        //
        // public virtual void RuntimeCheckLocalizedString() { }
    }
}
