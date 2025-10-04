using TMPro;
using UnityEngine;

namespace IsolarvHelperTools.Source.UI
{
    public abstract class DebugBaseField : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI label;

        public void SetLabel(string labelName)
        {
            label.text = labelName;
        }
    }
}