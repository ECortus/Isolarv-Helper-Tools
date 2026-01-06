using System;
using Plugins.GameDevUtils.Runtime.Extensions;
using UnityEngine;

namespace GameDevUtils.Runtime
{
    public abstract class AbstractResourceManager<T> : UnitySingleton<T> where T : MonoBehaviour
    {
        float resource;
        
        void SetResource(float value)
        {
            if (value.Equal(resource))
            {
                return;
            }
                
            onChanged?.Invoke();
            onValueChanged?.Invoke(value);
                
            if (value < 0)
            {
                this.resource = 0;
                return;
            }
                
            this.resource = value;
        }
        
        public void Plus(float amount)
        {
            var newAmount = resource + amount;
            SetResource(newAmount);
        }
        
        public void Reduce(float amount)
        {
            var newAmount = resource - amount;
            SetResource(newAmount);
        }
        
        public float GetValue()
        {
            return resource;
        }
        
        public int GetValueInt()
        {
            return (int)resource;
        }
        
        public void SetValue(float value)
        {
            SetResource(value);
        }

        public void ResetValue()
        {
            SetResource(0);
        }
        
        public event Action onChanged;
        public event Action<float> onValueChanged;
    }
}