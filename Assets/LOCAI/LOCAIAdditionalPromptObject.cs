using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LOCAI
{
    [AddComponentMenu("LOCAI/Other/LOCAI Additional Prompt")]
    public class LOCAIAdditionalPromptObject : MonoBehaviour
    {
        [Header("You can define additional instructions to AI for this object's children... Use English only in your prompt:")]
        [TextArea] public string AdditionalPrompt;
    }
}
