using UnityEngine;
using UnityEngine.UI;

namespace IsolarvHelperTools.Source.UI
{
    public class DebugButtonField : DebugBaseField
    {
        [SerializeField] private Button button;

        public delegate void ButtonAction();
        
        public void SetButtonAction(ButtonAction buttonAction)
        {
            button.onClick.AddListener(() =>
            {
                buttonAction();
            });
        }
    }
}