using System;
using UnityEngine;

namespace GameDevUtils.Runtime
{
    public abstract class AbstractCurrencyManager<T> : UnitySingleton<T> where T : MonoBehaviour
    {
        protected abstract string CurrencyName { get; }

        string prefsName => CurrencyName + "_Currency";

        int _value;
        int prefsValue
        {
            get => PlayerPrefs.GetInt(prefsName, 0);
            set => PlayerPrefs.SetInt(prefsName, value);
        }
        
        public int Value
        {
            get => prefsValue;
            set
            {
                if (value == prefsValue)
                {
                    return;
                }
                
                onChanged?.Invoke();
                onValueChanged?.Invoke(value);
                
                if (value < 0)
                {
                    prefsValue = 0;
                    return;
                }
                
                prefsValue = value;
            }
        }
        
        public event Action onChanged;
        public event Action<int> onValueChanged;
        
        public void AddCurrency(int amount)
        {
            Value += amount;
        }
        
        public void AddCurrency(float amount)
        {
            Value += Mathf.RoundToInt(amount);
        }
        
        public void RemoveCurrency(int amount)
        {
            Value -= amount;
        }
        
        public void RemoveCurrency(float amount)
        {
            Value -= Mathf.RoundToInt(amount);
        }

        public void Reset()
        {
            Value = 0;
        }
    }
}