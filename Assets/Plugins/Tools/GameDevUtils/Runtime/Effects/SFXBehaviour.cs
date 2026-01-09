using UnityEngine;

namespace GameDevUtils.Runtime.Effects
{
    public class SFXBehaviour : MonoBehaviour
    {
        public bool TurnObjectVisibility = false;
        
        [Space(5)]
        public bool CheckSources = true;   
        
        AudioSource[] audioSource;
        
        void Awake()
        {
            if (CheckSources)
            {
                audioSource = GetComponentsInChildren<AudioSource>();
            }
        }
        
        public void Play()
        {
            if (CheckSources)
            {
                foreach (var source in audioSource)
                {
                    source.Play();
                }
            }
            
            if (TurnObjectVisibility)
            {
                gameObject.SetActive(true);
            }
        }
        
        public void Stop()
        {
            if (CheckSources)
            {
                foreach (var source in audioSource)
                {
                    source.Stop();
                }
            }
            
            if (TurnObjectVisibility)
            {
                gameObject.SetActive(false);
            }
        }
    }
}