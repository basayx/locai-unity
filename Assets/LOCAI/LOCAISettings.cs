using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace LOCAI
{
    [CreateAssetMenu(fileName = "LOCAI Settings", menuName = "LOCAI/Settings", order = 0)]
    public class LOCAISettings : LOCAISingletonSettings<LOCAISettings>
    {
        public string APIKey;
        
        public bool IsActivated = true;

        public bool IsLogsEnable;
        
        [Space(10)]
        public SupportedLanguages OriginalLanguage;
        [SerializeField] private SupportedLanguages targetLanguage;
        public SupportedLanguages TargetLanguage => targetLanguage;
        public void SetTargetLanguage(SupportedLanguages language)
        {
            targetLanguage = language;
            _lastValidatedTargetLanguage = TargetLanguage;
            OnTargetLanguageChanged?.Invoke();
        }
        public List<SupportedLanguages> TargetSupportedLanguages = new List<SupportedLanguages>();

        public static UnityAction OnTargetLanguageChanged;
        private SupportedLanguages _lastValidatedTargetLanguage;
        
        private void OnValidate()
        {
            if (_lastValidatedTargetLanguage != TargetLanguage)
            {
                _lastValidatedTargetLanguage = TargetLanguage;
                OnTargetLanguageChanged?.Invoke();
            }
        }
        
        [Space(10)]
        [SerializeField] private List<FontSupport> targetSupportedFonts = new List<FontSupport>();
        public TMP_FontAsset GetSupportedFont() => targetSupportedFonts.Find(fontSupport => fontSupport.Language == TargetLanguage)?.FontAsset;
    }

    [System.Serializable]
    public class FontSupport
    {
        public SupportedLanguages Language;
        public TMP_FontAsset FontAsset;
    }
    
    public enum SupportedLanguages
    {
        English,
        Bulgarian,
        ChineseSimplified,
        ChineseTraditional,
        Czech,
        Danish,
        Dutch,
        Estonian,
        Finnish,
        French,
        German,
        Greek,
        Hebrew,
        Hungarian,
        Indonesian,
        Italian,
        Japanese,
        Korean,
        Latvian,
        Lithuanian,
        Norwegian,
        Polish,
        Portuguese,
        Romanian,
        Russian,
        Slovak,
        Slovenian,
        Spanish,
        Swedish,
        Thai,
        Turkish,
        Ukrainian,
        Vietnamese
    }
}