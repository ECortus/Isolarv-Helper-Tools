using System;
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
                DevUtilsSettingsSO.FindAsset("Dev Utils Settings Scriptable Object");
                
                var instance = DevUtilsSettingsSO.GetInstance;
                return instance;
#else
                return DevUtilsSettingsSO.GetInstance;
#endif
            }
        }
        
        public static bool EnableLoadingScreen
        {
            get => SettingsSO.EnableLoadingScreen;
            set => SettingsSO.EnableLoadingScreen = value;
        }

        public static LoadSceneMode LoadSceneMode
        {
            get => SettingsSO.LoadSceneMode;
            set => SettingsSO.LoadSceneMode = value;
        }
    }
}