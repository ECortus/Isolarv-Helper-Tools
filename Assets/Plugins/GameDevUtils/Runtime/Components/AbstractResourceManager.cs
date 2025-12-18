using System;
using UnityEngine;

namespace GameDevUtils.Runtime
{
    public abstract class AbstractResourceManager<T> : UnitySingleton<T> where T : MonoBehaviour
    {
        float _resource;
        float Resource
        {
            get => _resource;
            set
            {
                if (Mathf.Approximately(value, _resource))
                {
                    return;
                }
                
                onChanged?.Invoke();
                onValueChanged?.Invoke(value);
                
                if (value < 0)
                {
                    this._resource = 0;
                    return;
                }
                
                this._resource = value;
            }
        }
        
        public void Plus(float amount)
        {
            Resource += amount;
        }
        
        public void Reduce(float amount)
        {
            Resource -= amount;
        }
        
        public float GetValue()
        {
            return Resource;
        }
        
        public int GetValueInt()
        {
            return (int)Resource;
        }
        
        public void SetValue(float value)
        {
            Resource = value;
        }

        public void ResetValue()
        {
            Resource = 0;
        }
        
        public event Action onChanged;
        public event Action<float> onValueChanged;
    }
}