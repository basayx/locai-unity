using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using Object = UnityEngine.Object;

namespace LOCAI
{
    public static class LOCAIManager
    {
        public static IEnumerator SendGenerateContentRequest(SupportedLanguages localizationLanguage, string originalString, UnityAction<SupportedLanguages, string> onRespondSuccess, UnityAction onRespondFail = null, string additionalPrompt = "", float delay = 0f)
        {
            if (LOCAIConfigurations.Instance.UseGoogleCloudFunctionURL)
            {
                yield return GetTranslateViaGoogleCloudURLCoroutine(localizationLanguage, originalString, onRespondSuccess, onRespondFail, additionalPrompt, delay);
            }
            else if (LOCAIConfigurations.Instance.UseFirebaseFunctions)
            {
                yield return GetTranslateViaFirebaseFunctionCoroutine(localizationLanguage, originalString, onRespondSuccess, onRespondFail, additionalPrompt, delay);
            }
            else if (LOCAIConfigurations.Instance.UseKeyVaultLogic)
            {
                yield return GetTranslateViaGeminiAPICoroutine(LOCAIKeyVaultGateway.Instance.GetAPIKeyFromSecureServer(), localizationLanguage, originalString, onRespondSuccess, onRespondFail, additionalPrompt, delay);
            }
            else
            {
                yield return GetTranslateViaGeminiAPICoroutine(LOCAISettings.Instance.APIKey, localizationLanguage, originalString, onRespondSuccess, onRespondFail, additionalPrompt, delay);
            }
        }
        
        const string APIURL = "https://generativelanguage.googleapis.com/v1beta/models/gemini-1.5-flash-latest:generateContent?key=";

        private static IEnumerator GetTranslateViaGeminiAPICoroutine(string apiKey, SupportedLanguages localizationLanguage, string originalString, UnityAction<SupportedLanguages, string> onRespondSuccess, UnityAction onRespondFail = null, string additionalPrompt = "", float delay = 0f)
        {
            yield return new WaitForSeconds(delay);
            
            string prompt = $"Please translate '{originalString}' from {LOCAISettings.Instance.OriginalLanguage.ToString()} to {localizationLanguage}." + " You should translate not directly but semantically. You have to write the localized text alone as the answer and don't add additional quotation marks."
                + (string.IsNullOrEmpty(additionalPrompt) ? "" : ("Also consider that while translating: " + additionalPrompt));
            
            string url = APIURL + apiKey;
            string jsonRequestBody = $@"{{
                ""safetySettings"": [
                   {{
                       ""category"": ""7"",
                       ""threshold"": ""4""
                   }},
                   {{
                       ""category"": ""8"",
                       ""threshold"": ""4""
                   }},
                   {{
                       ""category"": ""9"",
                       ""threshold"": ""4""
                   }},
                   {{
                       ""category"": ""10"",
                       ""threshold"": ""4""
                   }}
               ],
               ""contents"": [
                   {{
                       ""parts"": [
                           {{
                               ""text"": ""{prompt}""
                           }}
                       ]
                   }}
               ]           
           }}";
            
            using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
            {
                byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonRequestBody);
                request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
                request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
                request.SetRequestHeader("Content-Type", "application/json");
                yield return request.SendWebRequest();
                if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
                {
                    Debug.LogError("Error: " + request.error);
                    onRespondFail?.Invoke();
                }
                else
                {
                    string responseText = request.downloadHandler.text;
                    // if (LOCAISettings.Instance.IsLogsEnable) Debug.Log(responseText);
                    Response responseData = JsonConvert.DeserializeObject<Response>(responseText);
                    string responseDataTextValue = responseData.candidates[0].content.parts[0].text.Replace(" \n", "");
                    if (LOCAISettings.Instance.IsLogsEnable) Debug.Log($"A localization process completed successfully: {originalString} => {responseDataTextValue}");
                    onRespondSuccess?.Invoke(localizationLanguage, string.IsNullOrEmpty(responseDataTextValue) ? originalString : responseDataTextValue);
                }
            }
        }

        private static IEnumerator GetTranslateViaGoogleCloudURLCoroutine(SupportedLanguages localizationLanguage, string originalString, UnityAction<SupportedLanguages, string> onRespondSuccess, UnityAction onRespondFail = null, string additionalPrompt = "", float delay = 0f)
        {
            yield return new WaitForSeconds(delay);

            string prompt = $"Please translate '{originalString}' from {LOCAISettings.Instance.OriginalLanguage.ToString()} to {localizationLanguage}." + " You should translate not directly but semantically. You have to write the localized text alone as the answer and don't add additional quotation marks."
                + (string.IsNullOrEmpty(additionalPrompt) ? "" : ("Also consider that while translating: " + additionalPrompt));
            
            string cloudFunctionUrl = LOCAIConfigurations.Instance.GoogleCloudFunctionURL;
            Dictionary<string, string> queryParameters = new Dictionary<string, string>();
            queryParameters.Add("MyPrompt", prompt);

            string queryString = "";
            foreach (KeyValuePair<string, string> param in queryParameters)
            {
                if (queryString != "")
                {
                    queryString += "&";
                }
                queryString += param.Key + "=" + UnityWebRequest.EscapeURL(param.Value);
            }
    
            using (var request = UnityWebRequest.Get(cloudFunctionUrl + "?" + queryString))
            {
                yield return request.SendWebRequest();

                if (request.result == UnityWebRequest.Result.ConnectionError)
                {
                    onRespondFail?.Invoke();
                    if (LOCAISettings.Instance.IsLogsEnable) Debug.LogError("Network Error\n" + localizationLanguage + "\n" + originalString + "\n" + request.error);
                }
                else if (request.result == UnityWebRequest.Result.ProtocolError)
                {
                    onRespondFail?.Invoke();
                    if (LOCAISettings.Instance.IsLogsEnable) Debug.LogError("HTTP Error\n" + localizationLanguage + "\n" + originalString + "\n" + request.responseCode + "\n" + request.downloadHandler.text);
                }
                else
                {
                    string responseText = request.downloadHandler.text;
                    if (LOCAISettings.Instance.IsLogsEnable) Debug.Log("RESPONSE: " + responseText);
                    onRespondSuccess?.Invoke(localizationLanguage, string.IsNullOrEmpty(responseText) ? originalString : responseText);
                }
            }
        }

        private static IEnumerator GetTranslateViaFirebaseFunctionCoroutine(SupportedLanguages localizationLanguage, string originalString, UnityAction<SupportedLanguages, string> onRespondSuccess, UnityAction onRespondFail = null, string additionalPrompt = "", float delay = 0f)
        {
            yield return new WaitForSeconds(delay);

            string prompt = $"Please translate '{originalString}' from {LOCAISettings.Instance.OriginalLanguage.ToString()} to {localizationLanguage}." + " You should translate not directly but semantically. You have to write the localized text alone as the answer and don't add additional quotation marks."
                + (string.IsNullOrEmpty(additionalPrompt) ? "" : ("Also consider that while translating: " + additionalPrompt));

#if LOCAI_FIREBASE            
            var func = FirebaseManager.Instance.Functions.GetHttpsCallable(LOCAIConfigurations.Instance.FirebaseFunctionName);
            var data = new Dictionary<string, object>();
            data["MyPrompt"] = prompt;

            var task = func.CallAsync(data).ContinueWithOnMainThread((callTask) => {
                if (callTask.IsFaulted) {
                    // The function unexpectedly failed.
                    onRespondFail?.Invoke();
                    Debug.Log("FAILED!\n" + String.Format("Error: {0}", callTask.Exception));
                    return;
                }

                // The function succeeded.
                var result = (IDictionary)callTask.Result.Data;
                onRespondSuccess?.Invoke(result["Respond"].ToString());
            });
            yield return new WaitUntil(() => task.IsCompleted);
#endif
        }

#if UNITY_EDITOR
        public static void SetDirty(Object target)
        {
            UnityEditor.EditorUtility.SetDirty(target);
        }
#endif
    }
}
