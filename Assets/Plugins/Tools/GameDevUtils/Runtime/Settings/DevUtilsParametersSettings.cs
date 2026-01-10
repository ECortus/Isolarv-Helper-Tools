using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameDevUtils.Runtime.Settings
{
    [Serializable]
    public sealed class DevUtilsParametersSettings
    {
        public static bool EnableLoadingScreen
        {
            get => PlayerPrefs.GetInt("Enable_Loading_Screen", 1) != 0;
            set => PlayerPrefs.SetInt("Enable_Loading_Screen", value ? 1 : 0);
        }

        public static LoadSceneMode LoadSceneMode
        {
            get => (LoadSceneMode)PlayerPrefs.GetInt("Load_Scene_Mode", 0);
            set => PlayerPrefs.SetInt("Load_Scene_Mode", (int)value);
        }
    }
}