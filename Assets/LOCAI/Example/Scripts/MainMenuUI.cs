using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LOCAI
{
    public class MainMenuUI : MonoBehaviour
    {
        [SerializeField] private GameObject mainMenuButtonsPanel;
        [SerializeField] private LanguageOptionsUI languageOptionsUI;
        [SerializeField] private GameplayUI gameplayUI;
        
        public void OnClickPlayButton()
        {
            gameObject.SetActive(false);
            gameplayUI.gameObject.SetActive(true);
        }

        public void OnClickSettingsButton()
        {
            mainMenuButtonsPanel.SetActive(false);
            languageOptionsUI.Show();
        }

        public void ShowButtons()
        {
            mainMenuButtonsPanel.SetActive(true);
        }
    }
}
