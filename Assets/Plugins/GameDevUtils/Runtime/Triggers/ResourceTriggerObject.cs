using UnityEngine;

namespace GameDevUtils.Runtime.Triggers
{
    public abstract class ResourceTriggerObject : TriggerObject
    {
        [Space(10)] 
        [SerializeField] private float resourceAmount = 5;
        [SerializeField] private bool disableObjectOnEnter = true;

        public void SetAmount(float customAmount)
        {
            resourceAmount = customAmount;
        }

        protected override void OnAwake()
        {
            base.OnAwake();
            OnTriggerEnterEvent += OnTriggerEnterMethod;
        }

        protected virtual void OnTriggerEnterMethod()
        {
            AddResource(resourceAmount);
            
            if (disableObjectOnEnter)
            {
                Disable();
            }
        }

        protected abstract void AddResource(float amount);
    }
}