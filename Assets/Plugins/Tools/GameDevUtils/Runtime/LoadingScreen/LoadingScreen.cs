using System;
using System.Collections;
using Cysharp.Threading.Tasks;
using GameDevUtils.Runtime.Scene;
using GameDevUtils.Runtime.UI;
using UnityEngine;

namespace GameDevUtils.Runtime
{
    public class LoadingScreen : UnitySingleton<LoadingScreen>
    {
        enum EDeltaType
        {
            Delta, Fixed, Unscaled
        }

        [Header("Fade parameters")] 
        [SerializeField] private EDeltaType deltaType = EDeltaType.Delta;
        [SerializeField] private float fadeInSpeed = 5f;
        [SerializeField] private float fadeOutSpeed = 5f;
        
        [Space(2)]
        [SerializeField] private float delayOnFadeIn = 0.5f;
        [SerializeField] private float delayOnFadeOut = 1f;

        public enum EScreenType
        {
            FirstInList, AsSceneIndex, Random
        }
        
        [Space(5)]
        [Header("Screen parameters")]
        public EScreenType screenType = EScreenType.FirstInList;
        
        ULoadingScreen screen;

        void Awake()
        {
            screen = GetComponentInChildren<ULoadingScreen>();
            ObjectHelper.DontDestroyOnLoad(this.gameObject);
        }
        
        public void LoadScene(SceneLoader.LoadSceneOperation operationDelegate)
        {
            StartCoroutine(OnLoadScene_Internal(operationDelegate));
        }

        IEnumerator OnLoadScene_Internal(SceneLoader.LoadSceneOperation operationDelegate)
        {
            screen.Show();
            
            yield return FadeIn(screen.UpdateBlackFade);
            yield return new WaitForSeconds(delayOnFadeIn);
            yield return FadeIn(screen.UpdateScreenFade);
            
            // Load Operation
            var operation = operationDelegate();
            while (operation.isDone == false)
            {
                var progress = Mathf.Clamp01(operation.progress / 0.9f);
                screen.UpdateScreenInfo(progress);
            
                yield return null;
            }
            
            yield return FadeOut(screen.UpdateScreenFade);
            yield return new WaitForSeconds(delayOnFadeOut);
            yield return FadeOut(screen.UpdateBlackFade);
            
            screen.Hide();
        }

        delegate void FadeMethodDelegate(float fade);

        IEnumerator FadeIn(FadeMethodDelegate methodDelegate)
        {
            float fade = 0f;
            while (fade < 1f)
            {
                fade += DeltaTime * fadeInSpeed;
                fade = Mathf.Clamp01(fade);
                
                methodDelegate(fade);
                yield return null;
            }
        }
        
        IEnumerator FadeOut(FadeMethodDelegate methodDelegate)
        {
            float fade = 1f;
            while (fade > 0f)
            {
                fade -= DeltaTime * fadeOutSpeed;
                fade = Mathf.Clamp01(fade);
                
                methodDelegate(fade);
                yield return null;
            }
        }

        float DeltaTime
        {
            get
            {
                float delta = 0;
                
                if (deltaType == EDeltaType.Delta)
                {
                    delta = Time.deltaTime;
                }
                else if (deltaType == EDeltaType.Fixed)
                {
                    delta = Time.fixedDeltaTime;
                }
                else if (deltaType == EDeltaType.Unscaled)
                {
                    delta = Time.unscaledDeltaTime;
                }
                else
                {
                    throw new NotImplementedException();
                }

                return delta;
            }
        }
    }
}