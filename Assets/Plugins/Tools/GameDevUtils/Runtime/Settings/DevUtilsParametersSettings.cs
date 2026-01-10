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
                if (!DevUtilsSettingsSO.HasInstance)
                {
                    DevUtilsSettingsSO.FindInAssets("Dev Utils Settings Scriptable Object");
                }

                return DevUtilsSettingsSO.GetInstance;
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