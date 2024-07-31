using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace LOCAI
{
    [RequireComponent(typeof(TMP_Text))]
    public class LOCAITMProObject : MonoBehaviour
    {
        private TMP_Text _tmpText;
        public TMP_Text TMPText
        {
            get
            {
                if (!_tmpText) _tmpText = GetComponent<TMP_Text>();
                return _tmpText;
            }
        }

        private TMP_FontAsset _defaultFont = null;
        
        public void InitializeFont()
        {
            if (_defaultFont == null) _defaultFont = TMPText.font;
            
            var supportedFont = LOCAISettings.Instance.GetSupportedFont();
            if (supportedFont)
                TMPText.font = supportedFont;
            else
                TMPText.font = _defaultFont;
        }
    }
}
