using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameDevUtils.Runtime.Settings
{
    public class DevUtilsSettingsSO : UnityScriptableSingleton<DevUtilsSettingsSO>
    {
        [ReadOnly] public bool EnableLoadingScreen = true;
        [ReadOnly] public LoadSceneMode LoadSceneMode = LoadSceneMode.Single;
    }
}