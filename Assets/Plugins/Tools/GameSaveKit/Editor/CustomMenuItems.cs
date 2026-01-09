using GameSaveKit.Editor.CustomWindows;
using GameSaveKit.Runtime.Saveable;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameSaveKit.Editor
{
    internal static class CustomMenuItems
    {
        [MenuItem("Tools/Isolarv/Game Save Kit/Instantiate supervisor", false, 10)]
        public static void InstantiateSaveableSupervisor()
        {
            if (GameObject.FindAnyObjectByType(typeof(SaveableSupervisor)))
            {
                Debug.LogWarning("[Save-Load Tool] Saveable Supervisor is already instantiated.");
                return;
            }
            
            var prefab = AssetDatabase.FindAssets("Saveable Supervisor")[0];
            var path = AssetDatabase.GUIDToAssetPath(prefab);
            var prefabObject = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            
            PrefabUtility.InstantiatePrefab(prefabObject);
            EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
        }
        
        [MenuItem("Tools/Isolarv/Game Save Kit/Settings", false, 15)]
        public static void ShowWindow()
        {
            EditorWindowUtils.ShowWindow<SaveLoadSettingsWindow>("Save & Load Settings");
        }
    }
}