using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LOCAI
{
    public class GameplayUI : MonoBehaviour
    {
        [SerializeField] private List<GameplayDialogue> dialogues = new List<GameplayDialogue>();
        private int _currentDialogueNo;

        [Header("Properties")]
        [SerializeField] private Color characterImagesDisplayColor = Color.white;
        [SerializeField] private Color characterImagesHideColor = Color.black;
        
        [Header("UI")]
        [SerializeField] private CanvasGroup dialoguePanelCanvasGroup;
        [SerializeField] private List<Image> characterImages = new List<Image>();
        [SerializeField] private List<TextMeshProUGUI> characterNameTexts = new List<TextMeshProUGUI>();
        [SerializeField] private List<TextMeshProUGUI> dialogueTexts = new List<TextMeshProUGUI>();
        private int _textUIElementIndex = -1;

        [SerializeField] private Button mainMenuButton;
        
        void Start()
        {
            mainMenuButton.gameObject.SetActive(false);
            dialoguePanelCanvasGroup.alpha = 0;
            ShowDialogue();
        }

        private void ShowDialogue()
        {
            if (_currentDialogueNo < dialogues.Count)
            {
                StartCoroutine(ShowDialogueCoroutine());
            }
            else
            {
                mainMenuButton.gameObject.SetActive(true);
                dialoguePanelCanvasGroup.gameObject.SetActive(false);
            }

            IEnumerator ShowDialogueCoroutine()
            {
                int oldTextUIElementIndex = _textUIElementIndex;
                _textUIElementIndex++;
                if (_textUIElementIndex >= dialogueTexts.Count) _textUIElementIndex = 0;
                int currentTextUIElementIndex = _textUIElementIndex;
                int nextTextUIElementIndex = _textUIElementIndex + 1;
                if (nextTextUIElementIndex >= dialogueTexts.Count) nextTextUIElementIndex = 0;

                var currentDialogue = dialogues[_currentDialogueNo];
                if (oldTextUIElementIndex < 0)
                {
                    characterNameTexts[currentTextUIElementIndex].text = currentDialogue.CharacterName;
                    dialogueTexts[currentTextUIElementIndex].text = currentDialogue.DialogueText;
                }
                
                if (oldTextUIElementIndex >= 0)
                {
                    StartCoroutine(TextCoroutine(characterNameTexts[oldTextUIElementIndex], false));
                    StartCoroutine(TextCoroutine(dialogueTexts[oldTextUIElementIndex], false));
                }
                
                var nextDialogue = _currentDialogueNo + 1 < dialogues.Count ? dialogues[_currentDialogueNo + 1] : null;
                if (nextDialogue != null)
                {
                    characterNameTexts[nextTextUIElementIndex].text = nextDialogue.CharacterName;
                    dialogueTexts[nextTextUIElementIndex].text = nextDialogue.DialogueText;
                }
                
                yield return new WaitForEndOfFrame();
                
                var unstableLOCAI = dialogueTexts[currentTextUIElementIndex].GetComponent<LOCAIUnstableRuntime>();
                while (unstableLOCAI && !unstableLOCAI.IsCurrentlyStable())
                {
                    yield return new WaitForEndOfFrame();
                }
                
                dialoguePanelCanvasGroup.alpha = 1;

                StartCoroutine(TextCoroutine(characterNameTexts[currentTextUIElementIndex], true));
                StartCoroutine(TextCoroutine(dialogueTexts[currentTextUIElementIndex], true));
                
                foreach (Image characterImage in characterImages)
                {
                    StartCoroutine(CharacterImageCoroutine(characterImage, (characterImage.gameObject.name == currentDialogue.CharacterName)));
                }
            } 
        }

        public void OnClickDialogueContinueButton()
        {
            _currentDialogueNo++;
            ShowDialogue();
        }

        IEnumerator TextCoroutine(TextMeshProUGUI targetText, bool isDisplaying)
        {
            float fadeDuration = isDisplaying ? 0.4f : 0.01f;
            Color color = targetText.color;
            float alpha = color.a;
            float targetAlpha = isDisplaying ? 1f : 0f;

            float durationTimeLeft = fadeDuration;
            while (durationTimeLeft > 0f)
            {
                var lerpValue = 1f - (durationTimeLeft / fadeDuration);
                alpha = Mathf.Lerp(alpha, targetAlpha,  lerpValue);

                color.a = alpha;
                targetText.color = color; 

                durationTimeLeft -= 1f * Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            
            color.a = targetAlpha;
            targetText.color = color; 
        }

        IEnumerator CharacterImageCoroutine(Image targetImage, bool isDisplaying)
        {
            float fadeDuration = 0.4f;
            Color targetColor = isDisplaying ? characterImagesDisplayColor : characterImagesHideColor;

            float durationTimeLeft = fadeDuration;
            while (durationTimeLeft > 0f)
            {
                var lerpValue = 1f - (durationTimeLeft / fadeDuration);
                targetImage.color = Color.Lerp(targetImage.color, targetColor,  lerpValue);

                targetImage.transform.localScale = Vector3.Lerp(targetImage.transform.localScale, Vector3.one * (isDisplaying ? 1.1f : 1f), lerpValue);
                
                durationTimeLeft -= 1f * Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
        }

        public void ResetScene()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        }
    }
    
    [System.Serializable]
    public class GameplayDialogue
    {
        public string CharacterName;
        public string DialogueText;
    }
}

