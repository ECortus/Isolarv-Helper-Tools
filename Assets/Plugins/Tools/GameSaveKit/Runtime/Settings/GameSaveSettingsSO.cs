using GameDevUtils.Runtime;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameSaveKit.Runtime.Settings
{
    public class GameSaveSettingsSO : UnityScriptableSingleton<GameSaveSettingsSO>
    {
        [ReadOnly] public bool LoadBehavioursOnAdding = true;
        [ReadOnly] public GameSaveParametersSettings.ESavingType SavingType = GameSaveParametersSettings.ESavingType.Json;
    }
}