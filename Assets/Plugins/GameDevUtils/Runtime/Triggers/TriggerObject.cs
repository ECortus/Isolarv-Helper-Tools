using System;
using Plugins.GameDevUtils.Runtime.Extensions;
using UnityEngine;

namespace GameDevUtils.Runtime.Triggers
{
    public class TriggerObject : MonoBehaviour
    {
        public enum EDimension
        {
            TwoDimension,
            ThreeDimension
        }
        
        public EDimension Dimension = EDimension.ThreeDimension;
        
        [Space(5)]
        public string Id = "trigger_object_sample_id";
        
        [Space(5)]
        public bool InEnterEvent = true;
        public bool InStayEvent = true;
        public bool InExitEvent = true;

        Rigidbody2D rb2D;
        Collider2D[] colliders2D;
        
        Rigidbody rb;
        Collider[] colliders;
        
        void Awake()
        {
            if (Dimension == EDimension.ThreeDimension)
            {
                Setup3D();
            }
            else if (Dimension == EDimension.TwoDimension)
            {
                Setup2D();
            }
            else
            {
                DebugHelper.LogError($"TriggerObject {gameObject.name}: Invalid dimension");
            }
        }

        void Setup2D()
        {
            rb2D = this.GetOrAddComponent<Rigidbody2D>();
            rb2D.bodyType = RigidbodyType2D.Kinematic;
            rb2D.simulated = false;
            
            colliders2D = GetComponentsInChildren<Collider2D>();
            if (colliders2D.Length > 0)
            {
                foreach (var col in colliders2D)
                {
                    col.isTrigger = true;
                    col.enabled = true;
                }
            }
            else
            {
                DebugHelper.LogError($"TriggerObject2D {gameObject.name}: No colliders found");
            }
        }

        void Setup3D()
        {
            rb = this.GetOrAddComponent<Rigidbody>();
            rb.isKinematic = true;
            
            colliders = GetComponentsInChildren<Collider>();
            if (colliders.Length > 0)
            {
                foreach (var col in colliders)
                {
                    col.isTrigger = true;
                    col.enabled = true;
                }
            }
            else
            {
                DebugHelper.LogError($"TriggerObject {gameObject.name}: No colliders found");
            }
        }
        
        void OnTriggerEnter(Collider other)
        {
            if (InEnterEvent)
            {
                OnTriggerEnterEvent?.Invoke(other);
            }
        }
        
        void OnTriggerStay(Collider other)
        {
            if (InStayEvent)
            {
                OnTriggerStayEvent?.Invoke(other);
            }
        }
        
        void OnTriggerExit(Collider other)
        {
            if (InExitEvent)
            {
                OnTriggerExitEvent?.Invoke(other);
            }
        }
        
        public event Action<Collider> OnTriggerEnterEvent;
        public event Action<Collider> OnTriggerStayEvent;
        public event Action<Collider> OnTriggerExitEvent;
    }
}