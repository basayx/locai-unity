using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace LOCAI
{
    public abstract class LOCAIConstant : LOCAISceneObject
    {
        protected override bool IsStoringLanguagesCorrect => base.IsStoringLanguagesCorrect;

        protected override void SetStoringLanguagesList()
        {
            base.SetStoringLanguagesList();
        }
    }
}
