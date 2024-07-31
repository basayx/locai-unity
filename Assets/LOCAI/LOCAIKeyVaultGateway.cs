using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LOCAI
{
    public class LOCAIKeyVaultGateway : MonoSingleton<LOCAIKeyVaultGateway>
    {
        ///You need to set your key vault logic and send the API key as result from this point.
        ///Check LOCAI Github page for more information about key vault.
        public string GetAPIKeyFromSecureServer()
        {
            string result = "";
            return string.IsNullOrEmpty(result) ? LOCAISettings.Instance.APIKey : result;
        }
    }
}
