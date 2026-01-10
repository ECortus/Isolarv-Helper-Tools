using GameDevUtils.Runtime;
using GameSaveKit.Editor.CustomWindows;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameDevUtils.Editor
{
    public static class CustomMenuItems
    {
        [MenuItem("Tools/Isolarv/Game Dev Utils/Settings", false, 10)]
        public static void ShowWindow()
        {
            EditorWindowUtils.ShowWindow<DevUtilsSettingsWindow>("Dev Utils Settings");
        }
        
        [MenuItem("Tools/Isolarv/Game Dev Utils/Instantiate default loading screen", false, 9)]
        public static void InstantiateSaveableSupervisor()
        {
            if (GameObject.FindAnyObjectByType(typeof(LoadingScreen)))
            {
                DebugHelper.LogWarning("Loading Screen is already instantiated.");
                return;
            }
            
            var prefab = AssetDatabase.FindAssets("Default Loading Screen")[0];
            var path = AssetDatabase.GUIDToAssetPath(prefab);
            var prefabObject = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            
            PrefabUtility.InstantiatePrefab(prefabObject);
            EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
        }
    }
}