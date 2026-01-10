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

        public static void FindInAssets(string name)
        {
            var prefab = AssetDatabase.FindAssets(name)[0];
            var path = AssetDatabase.GUIDToAssetPath(prefab);
            var prefabObject = AssetDatabase.LoadAssetAtPath<ScriptableObject>(path);
            
            _instance = prefabObject as T;
        }
        
        public void Init()
        {
            _instance = this as T;
        }
    }
}