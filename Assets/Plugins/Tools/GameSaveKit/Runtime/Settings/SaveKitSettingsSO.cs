using GameDevUtils.Runtime;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameSaveKit.Runtime.Settings
{
    public class SaveKitSettingsSO : UnityScriptableSingleton<SaveKitSettingsSO>
    {
        [ReadOnly] public bool LoadBehavioursOnAdding = true;
        [ReadOnly] public SaveKitParametersSettings.ESavingType SavingType = SaveKitParametersSettings.ESavingType.Json;
    }
}