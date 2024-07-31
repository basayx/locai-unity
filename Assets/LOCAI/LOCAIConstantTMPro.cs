using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Events;

namespace LOCAI
{
    [RequireComponent(typeof(LOCAITMProObject))]
    [AddComponentMenu("LOCAI/Constant/LOCAI TMPro (Constant)")]
    public class LOCAIConstantTMPro : LOCAIConstant
    {
        protected override string GetOriginalStringInitially() { return TMPObject.TMPText.text; }

        private LOCAITMProObject _tmpObject;
        public LOCAITMProObject TMPObject
        {
            get
            {
                if (!_tmpObject) _tmpObject = GetComponent<LOCAITMProObject>();
                return _tmpObject;
            }
        }

        protected override void ManualStart()
        {
            base.ManualStart();
            
            TMPObject.InitializeFont();
            TMPObject.TMPText.text = GetTargetLocalizedString();
        }
    }
}
