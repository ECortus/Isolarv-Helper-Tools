using UnityEditor;

#if UNITY_EDITOR
using UnityEditor.SceneManagement;
#endif

using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameDevUtils.Runtime
{
    public abstract class UnityScriptableSingleton<T> : ScriptableObject
        where T : ScriptableObject
    {
        static T _instance;
        
        public static bool HasInstance => _instance;
        
        public static T GetInstance => _instance;
        
#if UNITY_EDITOR
        
        public static T FindAsset(string name)
        {
            var prefab = AssetDatabase.FindAssets(name)[0];
            var path = AssetDatabase.GUIDToAssetPath(prefab);
            var prefabObject = AssetDatabase.LoadAssetAtPath<ScriptableObject>(path);
            
            return prefabObject as T;
        }
        
#endif
        
        public void Init()
        {
            if (_instance && _instance != this)
            {
                DebugHelper.LogError($"{typeof(T).Name} instance already exists. {typeof(T).Name} will not be initialized.");
                return;
            }
            
            _instance = this as T;
        }
    }
}