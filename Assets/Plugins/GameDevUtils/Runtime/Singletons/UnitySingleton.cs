using System;
using UnityEngine;

namespace GameDevUtils.Runtime
{
    public abstract class UnitySingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        static T _instance;
        
        void Awake()
        {
            if (_instance)
            {
                if (_instance != this)
                {
                    ObjectHelper.Destroy(this.gameObject);
                    return;
                }
            }
            else
            {
                _instance = this as T;
            }
            
            OnAwake();
        }

        protected virtual void OnAwake() { }

        public static bool HasInstance => _instance;

        public static bool HasOrFindInstance
        {
            get
            {
                TryFindObject(out _instance);
                return _instance;
            }
        }

        public static T GetInstance
        {
            get
            {
                TryFindObject(out _instance);
                
                if (!_instance)
                    DebugHelper.LogError("Not found instance of SINGLETON object!!!");

                return _instance;
            }
        }

        public static T GetOrCreateInstance
        {
            get
            {
                TryFindObject(out _instance);

                if (!_instance)
                    AddNewObject();

                return _instance;
            }
        }
        
        public static void DestroySingleton()
        {
            if (_instance)
            {
                ObjectHelper.Destroy(_instance.gameObject);
                _instance = null;
            }
        }

        static void SetInstance(T t)
        {
            _instance = t;
        }

        static void TryFindObject(out T instance)
        {
            if (_instance != null)
            {
                instance = _instance;
                return;
            }
            
            instance = (T)FindAnyObjectByType(typeof(T));
            SetInstance(instance);
        }

        static void AddNewObject()
        {
            GameObject go = new GameObject();

            var instance = go.AddComponent<T>();
            instance.name = "(singleton) " + typeof(T).ToString();

            SetInstance(instance);
        }
    }
}
