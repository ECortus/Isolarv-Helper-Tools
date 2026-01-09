using UnityEngine;

namespace GameDevUtils.Runtime
{
    public abstract class UnityScriptableSingleton<T> : ScriptableObject
        where T : ScriptableObject
    {
        static T _instance;
        
        public static bool HasInstance => _instance;
        
        public static T GetInstance => _instance;
        
        public void Init()
        {
            _instance = this as T;
        }
    }
}