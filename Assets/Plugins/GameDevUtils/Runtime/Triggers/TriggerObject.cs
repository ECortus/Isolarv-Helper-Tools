using System;
using GameDevUtils.Runtime.Extensions;
using Plugins.GameDevUtils.Runtime.Extensions;
using UnityEngine;

namespace GameDevUtils.Runtime.Triggers
{
    public class TriggerObject : MonoBehaviour
    {
        enum EDimension
        {
            TwoDimension,
            ThreeDimension
        }
        
        [SerializeField] private EDimension dimension = EDimension.ThreeDimension;
        
        [Space(5)]
        [SerializeField] private string id = "trigger_object_sample_id";
        
        [Space(5)]
        [SerializeField] private bool inEnterEvent = true;
        [SerializeField] private bool inStayEvent = true;
        [SerializeField] private bool inExitEvent = true;
        
        [Space(5)]
        [SerializeField] private LayerMask layerMask = ~0;

        Rigidbody2D rb2D;
        Collider2D[] colliders2D;
        
        Rigidbody rb;
        Collider[] colliders;
        
        public string Id => id;
        
        private void Awake()
        {
            if (dimension == EDimension.ThreeDimension)
            {
                Setup3D();
            }
            else if (dimension == EDimension.TwoDimension)
            {
                Setup2D();
            }
            else
            {
                DebugHelper.LogError($"TriggerObject {gameObject.name}: Invalid dimension");
            }
        }
        
        protected virtual void OnAwake() { }

        private void Start()
        {
            OnStart();
        }
        
        protected virtual void OnStart() { }

        protected void Enable()
        {
            gameObject.SetActive(true);
        }
        
        protected void Disable()
        {
            gameObject.SetActive(false);
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
            if (inEnterEvent)
            {
                if (other.IsSameMask(layerMask))
                {
                    OnTriggerEnterEvent?.Invoke();
                    OnTriggerEnterColliderEvent?.Invoke(other);
                }
            }
        }
        
        void OnTriggerStay(Collider other)
        {
            if (inStayEvent)
            {
                if (other.IsSameMask(layerMask))
                {
                    OnTriggerStayEvent?.Invoke();
                    OnTriggerStayColliderEvent?.Invoke(other);
                }
            }
        }
        
        void OnTriggerExit(Collider other)
        {
            if (inExitEvent)
            {
                if (other.IsSameMask(layerMask))
                {
                    OnTriggerExitEvent?.Invoke();
                    OnTriggerExitColliderEvent?.Invoke(other);
                }
            }
        }
        
        public event Action OnTriggerEnterEvent;
        public event Action OnTriggerStayEvent;
        public event Action OnTriggerExitEvent;
        
        public event Action<Collider> OnTriggerEnterColliderEvent;
        public event Action<Collider> OnTriggerStayColliderEvent;
        public event Action<Collider> OnTriggerExitColliderEvent;
    }
}