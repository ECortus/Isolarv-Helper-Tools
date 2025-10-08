using TMPro;
using UnityEngine;

namespace IsolarvHelperTools.Runtime.UI
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