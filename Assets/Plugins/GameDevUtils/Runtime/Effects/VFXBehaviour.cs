using System;
using UnityEngine;

namespace GameDevUtils.Runtime.Effects
{
    public class VFXBehaviour : MonoBehaviour
    {
        public bool TurnObjectVisibility = false;
        
        [Space(5)]
        public bool CheckParticleSystems = true;
        public bool CheckTrailRenderers = true;
        
        ParticleSystem[] particleSystems;
        TrailRenderer[] trailRenderers;

        private void Awake()
        {
            if (CheckParticleSystems)
            {
                particleSystems = GetComponentsInChildren<ParticleSystem>();
            }
            
            if (CheckTrailRenderers)
            {
                trailRenderers = GetComponentsInChildren<TrailRenderer>();
            }
        }

        public void Play()
        {
            if (CheckParticleSystems)
            {
                foreach (var particle in particleSystems)
                {
                    particle.Play();
                }
            }

            if (CheckTrailRenderers)
            {
                foreach (var trail in trailRenderers)
                {
                    trail.Clear();
                    trail.enabled = true;
                }
            }
            
            if (TurnObjectVisibility)
            {
                gameObject.SetActive(true);
            }
        }

        public void Stop()
        {
            if (CheckParticleSystems)
            {
                foreach (var particle in particleSystems)
                {
                    particle.Stop();
                }
            }

            if (CheckTrailRenderers)
            {
                foreach (var trail in trailRenderers)
                {
                    trail.Clear();
                    trail.enabled = false;
                }
            }
            
            if (TurnObjectVisibility)
            {
                gameObject.SetActive(false);
            }
        }
    }
}