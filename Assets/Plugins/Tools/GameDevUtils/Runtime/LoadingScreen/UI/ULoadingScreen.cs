using UnityEngine;
using UnityEngine.UI;

namespace GameDevUtils.Runtime.UI
{
    public class ULoadingScreen : MonoBehaviour
    {
        [SerializeField] private GameObject root;
        [SerializeField] private UScreenCollection screenCollection;

        [Space(5)] 
        [SerializeField] private CanvasGroup blackGroup;
        [SerializeField] private CanvasGroup screenGroup;

        Slider slider;
        
        public void Show()
        {
            screenCollection.ShowScreen();
            slider = screenCollection.GetSlider();
            
            UpdateBlackFade(0);
            UpdateScreenFade(0);
            UpdateScreenInfo(0);
            
            root.SetActive(true);
        }

        public void UpdateBlackFade(float fade)
        {
            blackGroup.alpha = fade;
        }
        
        public void UpdateScreenFade(float fade)
        {
            screenGroup.alpha = fade;
        }

        public void UpdateScreenInfo(float progressPercentage)
        {
            slider.value = progressPercentage;
        }

        public void Hide()
        {
            root.SetActive(false);
        }
    }
}