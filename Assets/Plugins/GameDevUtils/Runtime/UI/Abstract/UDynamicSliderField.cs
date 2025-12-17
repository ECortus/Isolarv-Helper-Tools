using UnityEngine;
using UnityEngine.UI;

namespace GameDevUtils.Runtime.UI.Abstract
{
    public abstract class UDynamicSliderField : UDynamicField
    {
        [Space(5)]
        [SerializeField] private Slider slider;

        protected override void UpdateField()
        {
            UpdateSlider();
        }
        
        protected abstract float GetSliderValue();
        
        void UpdateSlider()
        {
            slider.value = GetSliderValue();
        }
    }
}