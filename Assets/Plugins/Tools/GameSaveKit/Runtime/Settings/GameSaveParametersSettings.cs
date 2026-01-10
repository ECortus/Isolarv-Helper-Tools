using System;
using UnityEngine;

namespace GameSaveKit.Runtime.Settings
{
    [System.Serializable]
    public sealed class GameSaveParametersSettings
    {
        [Serializable]
        public enum ESavingType
        {
            Json, Binary
        }
        
        static GameSaveSettingsSO SettingsSO
        {
            get
            {
                if (!GameSaveSettingsSO.HasInstance)
                {
                    GameSaveSettingsSO.FindInAssets("Save Kit Settings Scriptable Object");
                }

                return GameSaveSettingsSO.GetInstance;
            }
        }
        
        public static bool LoadBehavioursOnAdding
        {
            get => SettingsSO.LoadBehavioursOnAdding;
            set => SettingsSO.LoadBehavioursOnAdding = value;
        }
        
        public static ESavingType SavingType
        {
            get => SettingsSO.SavingType;
            set => SettingsSO.SavingType = value;
        }
    }
}