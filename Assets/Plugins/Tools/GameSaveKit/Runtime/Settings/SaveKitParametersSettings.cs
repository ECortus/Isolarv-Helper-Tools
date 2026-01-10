using System;
using UnityEngine;

namespace GameSaveKit.Runtime.Settings
{
    [System.Serializable]
    public sealed class SaveKitParametersSettings
    {
        [Serializable]
        public enum ESavingType
        {
            Json, Binary
        }

        static SaveKitSettingsSO SettingsSo
        {
            get
            {
#if UNITY_EDITOR
                SaveKitSettingsSO.FindAsset("Save Kit Settings Scriptable Object");
                
                var instance = SaveKitSettingsSO.GetInstance;
                return instance;
#else
                return SaveKitSettingsSO.GetInstance;
#endif
            }
        }
        
        public static bool LoadBehavioursOnAdding
        {
            get => SettingsSo.LoadBehavioursOnAdding;
            set => SettingsSo.LoadBehavioursOnAdding = value;
        }
        
        public static ESavingType SavingType
        {
            get => SettingsSo.SavingType;
            set => SettingsSo.SavingType = value;
        }
    }
}