using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace LOCAI
{
    public class LanguageOptionsUI : MonoBehaviour
    {
        [SerializeField] private MainMenuUI mainMenuUI;
        [SerializeField] private TextMeshProUGUI languageText;

        private void Start()
        {
            RefreshInfoText(LOCAISettings.Instance.TargetLanguage);
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        private void Hide()
        {
            gameObject.SetActive(false);
        }

        void RefreshInfoText(SupportedLanguages language)
        {
            languageText.text = $"{language}";
        }
        
        public void OnClickLanguageButton(int supportedLanguageIndex)
        {
            SupportedLanguages selectedLanguage = supportedLanguageIndex < 0
                ? LOCAISettings.Instance.OriginalLanguage
                : LOCAISettings.Instance.TargetSupportedLanguages[supportedLanguageIndex];
            RefreshInfoText(selectedLanguage);
            LOCAISettings.Instance.SetTargetLanguage(selectedLanguage);
        }

        public void OnClickBackButton()
        {
            mainMenuUI.ShowButtons();
            Hide();
        }
    }
}
