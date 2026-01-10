using System.Collections.Generic;
using System.Linq;
using GameDevUtils.Runtime;
using GameSaveKit.Editor.CustomWindows;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace GameDevUtils.Editor
{
    public static class CustomMenuItems
    {
        [MenuItem("Tools/Isolarv/Game Dev Utils/Settings", false, 10)]
        public static void ShowWindow()
        {
            EditorWindowUtils.ShowWindow<DevUtilsSettingsWindow>("Dev Utils Settings");
        }
        
        [MenuItem("Tools/Isolarv/Game Dev Utils/Instantiate default loading screen", false, 8)]
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

        [MenuItem("Tools/Isolarv/Game Dev Utils/Copy installer into project context", false, 9)]
        public static void CopyInstallerIntoProjectContext()
        {
            var prefabObject = Resources.Load<GameObject>("ProjectContext");
            if (!prefabObject)
            {
                DebugHelper.LogError("Context not instantiated.");
                return;
            }
            
            var installerPrefab = AssetDatabase.FindAssets("Dev Utils Mono Installer")[0];
            var installerPath = AssetDatabase.GUIDToAssetPath(installerPrefab);
            var installerPrefabObject = AssetDatabase.LoadAssetAtPath<GameObject>(installerPath);
            
            ProjectContext context = prefabObject.GetComponent<ProjectContext>(); 
            MonoInstaller installer = installerPrefabObject.GetComponent<MonoInstaller>();

            if (context.InstallerPrefabs.Contains(installer))
            {
                DebugHelper.Log("Context already contains DevUtilsMonoInstaller");
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