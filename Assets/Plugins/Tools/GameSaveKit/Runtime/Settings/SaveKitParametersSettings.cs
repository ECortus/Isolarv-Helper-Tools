using System;
using UnityEditor;
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

        static SaveKitSettingsSO SettingsSO
        {
            get
            {
#if UNITY_EDITOR
                var instance = SaveKitSettingsSO.FindAsset("Save Kit Settings Scriptable Object");
                return instance;
#else
                return SaveKitSettingsSO.GetInstance;
#endif
            }
        }
        
        public static bool LoadBehavioursOnAdding
        {
            get => SettingsSO.LoadBehavioursOnAdding;
#if UNITY_EDITOR
            set
            {
                var so = SettingsSO;
                
                so.LoadBehavioursOnAdding = value;
                SetObjectDirtyAndSave(so);
            }
#endif
        }
        
        public static ESavingType SavingType
        {
            get => SettingsSO.SavingType;
#if UNITY_EDITOR
            set
            {
                var so = SettingsSO;
                
                so.SavingType = value;
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