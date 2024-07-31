using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace LOCAI
{
    public class LOCAIRuntimeManager : LOCAIMonoSingleton<LOCAIRuntimeManager>
    {
        private bool _isSettingsChecked = false;
        private bool _isSettingsActivated = true;

        public bool İsSettingsActivated
        {
            get
            {
                if (!_isSettingsChecked)
                {
                    _isSettingsActivated = LOCAISettings.Instance.IsActivated;
                    _isSettingsChecked = true;
                }
                return _isSettingsActivated;
            }
        }

        protected override void Init()
        {
            base.Init();

            _isSettingsChecked = false;
        }
    }
}
