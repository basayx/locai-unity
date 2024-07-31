using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LOCAI
{
    [CreateAssetMenu(fileName = "LOCAI Data Holder", menuName = "LOCAI/Data Holder", order = 1)]
    public class LOCAIDataHolder : LOCAISingletonSettings<LOCAIDataHolder>
    {
        public List<LOCAIData> Datas = new List<LOCAIData>();
        public int RemovedAndStillEmptyDatasCount;
        
        public int AddNewDataAndGetIndex(string originalData)
        {
            int index = -1;
            if (RemovedAndStillEmptyDatasCount > 0)
            {
                var emptyDataIndex = Datas.FindLastIndex(data => data.IsEmpty);
                if (emptyDataIndex >= 0)
                {
                    Datas.RemoveAt(emptyDataIndex);
                    Datas.Insert(emptyDataIndex, new LOCAIData(originalData));
                    RemovedAndStillEmptyDatasCount--;
                    index = emptyDataIndex;
                }
            }

            if (index < 0)
            {
                index = Datas.Count;
                Datas.Add(new LOCAIData(originalData));
            }

            LOCAIManager.SetDirty(this);
            
            return index;
        }

        public LOCAIData GetData(int dataIndex)
        {
            return Datas[dataIndex];
        }

        public void RemoveData(int dataIndex)
        {
            if (dataIndex >= Datas.Count || dataIndex < 0)
            {
                Debug.LogWarning("Missing data index sent form a component that tried to remove...");
                return;
            }
            
            Datas[dataIndex].IsEmpty = true;
            Datas[dataIndex].OriginalString = "";
            Datas[dataIndex].LocalizedLanguages.Clear();
            Datas[dataIndex].LocalizedStrings.Clear();
            RemovedAndStillEmptyDatasCount++;
            
            LOCAIManager.SetDirty(this);
        }

        public void ResetAll()
        {
            Datas.Clear();
            RemovedAndStillEmptyDatasCount = 0;
        }

        [ContextMenu(nameof(Log))]
        private void Log()
        {
            string log = "";
            foreach (LOCAIData data in Datas)
            {
                log += data.OriginalString + "\n";
                foreach (SupportedLanguages language in data.LocalizedLanguages)
                {
                    log += language.ToString() + " = " + data.GetLocalizedString(language) + "\n\n";
                }
            }
            Debug.Log(log);
        }
    }

    [System.Serializable]
    public class LOCAIData
    {
        public bool IsEmpty = false;
        [TextArea] public string OriginalString;
        public List<SupportedLanguages> LocalizedLanguages = new List<SupportedLanguages>();
        public List<string> LocalizedStrings = new List<string>();
        public LOCAIData(string originalString)
        {
            IsEmpty = false;
            OriginalString = originalString;
            AddLocalization(LOCAISettings.Instance.OriginalLanguage, OriginalString);
        }

        public string GetTargetLocalizedString()
        {
            SupportedLanguages currentLanguage = LOCAISettings.Instance.TargetLanguage;
            return GetLocalizedString(currentLanguage);
        }
        public string GetLocalizedString(SupportedLanguages targetLanguage)
        {
            int languageIndex = LocalizedLanguages.IndexOf(targetLanguage);
            if (languageIndex < 0) throw new Exception($"Tried to get localized string from not validated object!\nObject's Data Index: {LOCAIDataHolder.Instance.Datas.IndexOf(this)}" +
                                                       $"\nTry manually validated the object if necessary...");
            return LocalizedStrings[languageIndex];
        }

        public void AddLocalization(SupportedLanguages language, string localizedString)
        {
            if (LocalizedLanguages.Contains(language))
            {
                LocalizedStrings[LocalizedLanguages.IndexOf(language)] = localizedString;
            }
            else
            {
                LocalizedLanguages.Add(language);
                LocalizedStrings.Add(localizedString);
            }
        }
    }
}