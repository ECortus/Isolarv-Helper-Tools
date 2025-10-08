using UnityEngine;

namespace IsolarvHelperTools.Runtime.UI
{
    public class DebugFieldManager : MonoBehaviour
    {
        [SerializeField] private Transform contentParent;
        
        [Space(5)]
        [SerializeField] private DebugButtonField buttonPrefab;
        [SerializeField] private DebugToggleField togglePrefab;
        
        public void RegisterDebugButton(string label, DebugButtonField.ButtonAction buttonAction)
        {
            DebugButtonField button = ObjectInstantiator.InstantiatePrefabForComponent(buttonPrefab, contentParent);
            button.SetLabel(label);
            button.SetButtonAction(buttonAction);
        }
        
        public void RegisterDebugToggle(string label, DebugToggleField.ToggleAction toggleAction, bool defaultValue)
        {
            DebugToggleField toggle = ObjectInstantiator.InstantiatePrefabForComponent(togglePrefab, contentParent);
            toggle.SetLabel(label);
            toggle.SetToggleAction(toggleAction);
            toggle.SetToggleValue(defaultValue);
        }
    }
}