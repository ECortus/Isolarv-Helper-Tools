using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameDevUtils.Runtime.Settings
{
    [Serializable]
    public sealed class DevUtilsParametersSettings
    {
        static DevUtilsSettingsSO SettingsSO
        {
            get
            {
#if UNITY_EDITOR
                var instance = DevUtilsSettingsSO.FindAsset("Dev Utils Settings Scriptable Object");
                return instance;
#else
                return DevUtilsSettingsSO.GetInstance;
#endif
            }
        }
        
        public static bool EnableLoadingScreen
        {
            get => SettingsSO.EnableLoadingScreen;
#if UNITY_EDITOR
            set
            {
                var so = SettingsSO;
                
                so.EnableLoadingScreen = value;
                SetObjectDirtyAndSave(so);
            }
#endif
            
        }

        public static LoadSceneMode LoadSceneMode
        {
            get => SettingsSO.LoadSceneMode;
#if UNITY_EDITOR
            set
            {
                var so = SettingsSO;
                
                so.LoadSceneMode = value;
                SetObjectDirtyAndSave(so);
            }
#endif
        }

#if UNITY_EDITOR
        static void SetObjectDirtyAndSave(ScriptableObject obj)
        {
            EditorUtility.SetDirty(obj);
            AssetDatabase.SaveAssetIfDirty(obj);
        }
#endif
    }
}