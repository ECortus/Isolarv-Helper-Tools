using UnityEngine;

namespace GameDevUtils.Runtime
{
    public abstract class UnitySingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;

        public static bool HasInstance => _instance;

        public static bool HasOrFindInstance
        {
            get
            {
                if (!_instance)
                    TryFindObject();

                return _instance;
            }
        }

        public static T GetInstance
        {
            get
            {
                if (!_instance)
                {
                    TryFindObject();

                    if (!_instance)
                        DebugHelper.LogError("Not found instance of SINGLETON object!!!");
                }

                return _instance;
            }
        }

        public static T GetOrCreateInstance
        {
            get
            {
                if (!_instance)
                {
                    TryFindObject();

                    if (!_instance)
                        AddNewObject();
                }

                return _instance;
            }
        }

        static void SetInstance(T t)
        {
            _instance = t;
        }

        static void TryFindObject()
        {
            var instance = (T)FindAnyObjectByType(typeof(T));
            SetInstance(instance);
        }

        static void AddNewObject()
        {
            GameObject go = new GameObject();

            var instance = go.AddComponent<T>();
            instance.name = "(singleton) " + typeof(T).ToString();

            SetInstance(instance);
        }

        public static void DestroySingleton()
        {
            if (_instance)
            {
                Destroy(_instance.gameObject);
                _instance = null;
            }
        }
    }
}
