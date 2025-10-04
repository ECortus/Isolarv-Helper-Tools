using UnityEngine;
using UnityEngine.UI;

namespace IsolarvHelperTools.Source.UI
{
    public class DebugToggleField : DebugBaseField
    {
        [SerializeField] private Toggle toggle;
        
        public delegate void ToggleAction(bool value);
        
        public void SetToggleAction(ToggleAction toggleAction)
        {
            toggle.onValueChanged.AddListener((value) =>
            {
                toggleAction(value);
            });
        }
        
        public void SetToggleValue(bool value)
        {
            toggle.isOn = value;
        }
    }
}