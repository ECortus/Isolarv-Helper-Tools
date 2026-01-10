using System.Collections.Generic;
using System.Linq;
using GameSaveKit.Editor.CustomWindows;
using GameSaveKit.Runtime.Saveable;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

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
        
        [MenuItem("Tools/Isolarv/Game Save Kit/Copy installer into project context", false, 12)]
        public static void CopyInstallerIntoProjectContext()
        {
            var prefabObject = Resources.Load<GameObject>("ProjectContext");
            if (!prefabObject)
            {
                Debug.LogError("[Save-Load Tool] Context not instantiated.");
                return;
            }
            
            var installerPrefab = AssetDatabase.FindAssets("Save Kit Mono Installer")[0];
            var installerPath = AssetDatabase.GUIDToAssetPath(installerPrefab);
            var installerPrefabObject = AssetDatabase.LoadAssetAtPath<GameObject>(installerPath);
            
            ProjectContext context = prefabObject.GetComponent<ProjectContext>(); 
            MonoInstaller installer = installerPrefabObject.GetComponent<MonoInstaller>();

            if (context.InstallerPrefabs.Contains(installer))
            {
                Debug.Log("[Save-Load Tool] Context already contains SaveKitMonoInstaller");
                return;
            }
            
            var prefabs = new List<MonoInstaller>();
            prefabs.AddRange(context.InstallerPrefabs);
            
            context.InstallerPrefabs = prefabs.Append(installer);
            
            EditorUtility.SetDirty(context);
            AssetDatabase.SaveAssetIfDirty(context);
        }
    }
}