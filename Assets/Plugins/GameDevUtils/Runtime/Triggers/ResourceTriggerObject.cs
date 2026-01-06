using UnityEngine;

namespace GameDevUtils.Runtime.Triggers
{
    public abstract class ResourceTriggerObject : TriggerObject
    {
        [Space(10)] 
        [SerializeField] private float resourceAmount = 5;

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
        }

        protected abstract void AddResource(float amount);
    }
}