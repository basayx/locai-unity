using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace LOCAI
{
    public abstract class LOCAISceneObject : MonoBehaviour
    {
        [HideInInspector] public int _dataIndex = -1;

        [HideInInspector] public string _originalString = string.Empty;
        protected virtual string OriginalString
        {
            get
            {
                if (string.IsNullOrEmpty(_originalString)) _originalString = GetOriginalStringInitially();
                return _originalString;
            }
        }
        protected abstract string GetOriginalStringInitially();

        [HideInInspector] public List<SupportedLanguages> _storingLanguages = new List<SupportedLanguages>();
        protected virtual bool IsStoringLanguagesCorrect => _storingLanguages.SequenceEqual(LOCAISettings.Instance.TargetSupportedLanguages);

        public UnityAction<SupportedLanguages, string> OnDefineLocalizationsProcessFinished;
    
        protected bool _isInLocalizationProcess;
        
        private bool _isSubbedToTargetLanguage;

        public bool IsValidated;
        
        private void OnValidate()
        {
            if (IsValidated)
                return;
            
            CheckDefineLocalization();
            
            IsValidated = true;
            LOCAIManager.SetDirty(this);
        }

        void SubToTargetLanguage()
        {
            if (!_isSubbedToTargetLanguage)
            {
                LOCAISettings.OnTargetLanguageChanged += OnTargetLanguageChanged;
                _isSubbedToTargetLanguage = true;
            }
        }
        void UnsubToTargetLanguage()
        {
            if (_isSubbedToTargetLanguage)
            {
                LOCAISettings.OnTargetLanguageChanged -= OnTargetLanguageChanged;
                _isSubbedToTargetLanguage = false;
            }
        }

        private void OnDestroy()
        {
            UnsubToTargetLanguage();
        }

        private void OnApplicationQuit()
        {
            UnsubToTargetLanguage();
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus)
                UnsubToTargetLanguage();
        }

        void OnTargetLanguageChanged()
        {
            CheckDefineLocalization();
        }

        private void Start()
        {
            LOCAIRuntimeManager.Instance.StartCoroutine(LateStartCoroutine());
            IEnumerator LateStartCoroutine()
            {
                yield return new WaitForEndOfFrame();
                
                SubToTargetLanguage();
                ManualStart();
            }
        }
        
        protected virtual void ManualStart() { }

        protected virtual void CheckDefineLocalization()
        {
            if (_dataIndex < 0)
            {
                _dataIndex = LOCAIDataHolder.Instance.AddNewDataAndGetIndex(OriginalString);

                DefineLocalizations();
                
                LOCAIManager.SetDirty(this);
                
                return;
            }

            if (!IsStoringLanguagesCorrect)
            {
                DefineLocalizations();
                
                LOCAIManager.SetDirty(this);
            }
            else if (Application.isPlaying)
            {
                ManualStart();
            }
        }
        
        protected virtual void SetStoringLanguagesList()
        {
            _storingLanguages = new List<SupportedLanguages>(LOCAISettings.Instance.TargetSupportedLanguages);
        }
        
        protected void DefineLocalizations()
        {
            if (_isInLocalizationProcess)
            {
                Debug.LogWarning($"Localization define attempted while an object still in another process...\nObject Name: {gameObject.name}");
                return;
            }
            _isInLocalizationProcess = true;
            
            SetStoringLanguagesList();
            
            LOCAIRuntimeManager.Instance.StartCoroutine(DefineLocalizationsCoroutine());
            IEnumerator DefineLocalizationsCoroutine()
            {
                var data = LOCAIDataHolder.Instance.GetData(_dataIndex);
                string lastLocalizedString = OriginalString;
                SupportedLanguages lastLocalizedLanguage = LOCAISettings.Instance.OriginalLanguage;

                var additionalPrompt = GetComponentInParent<LOCAIAdditionalPromptObject>(true)?.AdditionalPrompt;
                
                foreach (SupportedLanguages storingLanguage in _storingLanguages)
                {
                    lastLocalizedLanguage = storingLanguage;

                    int repeatingDelay = 0;
                    bool isDefined = false;
                    
                    if (!data.LocalizedLanguages.Contains(storingLanguage))
                    {
                        LOCAIRuntimeManager.Instance.StartCoroutine(LOCAIManager.SendGenerateContentRequest(storingLanguage, OriginalString, OnTranslateSuccess, OnTranslateFail, additionalPrompt));
                    }
                    else
                    {
                        lastLocalizedString = data.LocalizedStrings[data.LocalizedLanguages.IndexOf(storingLanguage)];
                        isDefined = true;
                    }
                    
                    void OnTranslateSuccess(SupportedLanguages language, string localizedString)
                    {
                        data.AddLocalization(storingLanguage, localizedString);
                        lastLocalizedString = localizedString;
                        isDefined = true;
                    }
                    
                    void OnTranslateFail()
                    {
                        repeatingDelay += 10;
                        if (repeatingDelay < 60)
                            LOCAIRuntimeManager.Instance.StartCoroutine(LOCAIManager.SendGenerateContentRequest(storingLanguage, OriginalString, OnTranslateSuccess, OnTranslateFail, additionalPrompt, repeatingDelay));
                        else
                        {
                            Debug.LogError($"Localization attempt failed for an object after few attempt!\nObject Name: {gameObject.name}" +
                                           $"\nTry to remove the component and reassign after few minutes...");
                        }
                    }
                    
                    while (!isDefined)
                    {
                        yield return null;
                    }

                    yield return null;
                }

                yield return null;
                
                _isInLocalizationProcess = false;
                DefineLocalizationsProcessFinished(lastLocalizedLanguage, lastLocalizedString);
            }
        }

        protected virtual void DefineLocalizationsProcessFinished(SupportedLanguages lastLocalizedLanguage, string lastLocalizedString)
        {
            OnDefineLocalizationsProcessFinished?.Invoke(lastLocalizedLanguage, lastLocalizedString);
        }

        protected LOCAIData GetData()
        {
            if (_dataIndex < 0) throw new Exception($"Tried to get data from not validated object!\nObject Name: {gameObject.name}" +
                                                    $"\nTry manually validated the object if necessary...");
            return LOCAIDataHolder.Instance.GetData(_dataIndex);
        }

        public virtual string GetTargetLocalizedString() => LOCAIRuntimeManager.Instance.İsSettingsActivated ? GetData().GetTargetLocalizedString() : OriginalString;

        [ContextMenu(nameof(RemoveTheComponent))]
        public void RemoveTheComponent()
        {
            if (_dataIndex >= 0)
            {
                LOCAIDataHolder.Instance.RemoveData(_dataIndex);
                _dataIndex = -1;
                DestroyImmediate(this);
            }
        }
        
        [ContextMenu(nameof(LogOriginalString))]
        public void LogOriginalString()
        {
            Debug.Log(OriginalString);
        }
    }
}
