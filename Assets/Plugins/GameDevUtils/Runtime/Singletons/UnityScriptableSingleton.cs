using UnityEngine;

namespace GameDevUtils.Runtime
{
    public abstract class UnityScriptableSingleton<T> : ScriptableObject
        where T : ScriptableObject
    {
        public static T Instance { get; private set; }
        
        public void Init()
        {
            Instance = this as T;
        }
    }
}