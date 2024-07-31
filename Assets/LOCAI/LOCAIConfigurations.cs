using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LOCAI
{
    [CreateAssetMenu(fileName = "LOCAI Configurations", menuName = "LOCAI/Configurations", order = 2)]
    public class LOCAIConfigurations : LOCAISingletonSettings<LOCAIConfigurations>
    {
        public bool UseGoogleCloudFunctionURL;
        public string GoogleCloudFunctionURL;
        
        public bool UseFirebaseFunctions;
        public string FirebaseFunctionName;
        
        public bool UseKeyVaultLogic;
    }
}
