using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameDevUtils.Runtime.UI.Abstract
{
    public abstract class UDynamicSliderField : UDynamicField
    {
        [Space(5)]
        [SerializeField] private Slider slider;
        [SerializeField] private TMP_Text label;

        protected override void UpdateField()
        {
            UpdateSlider();
            UpdateLabel();
        }
        
        protected abstract float GetSliderValue();
        
        void UpdateSlider()
        {
            slider.value = GetSliderValue();
        }
        
        protected virtual string GetLabelText() { return ""; }
        
        void UpdateLabel()
        {
            label.text = GetLabelText();
        }
    }
}