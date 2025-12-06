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
        
        public static ESavingType SavingType
        {
            get => (ESavingType)PlayerPrefs.GetInt("Saving_Type", 0);
            set => PlayerPrefs.SetInt("Saving_Type", (int)value);
        }
        
        public static bool LoadBehavioursOnAdding
        {
            get => PlayerPrefs.GetInt("Load_Behaviour_On_Adding", 0) != 0;
            set => PlayerPrefs.SetInt("Load_Behaviour_On_Adding", value ? 1 : 0);
        }
    }
}