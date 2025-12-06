using System;
using UnityEngine;

namespace LocalizationModule.Runtime
{
    [RequireComponent(typeof(LocalizationManager))]
    internal class LocalizationSettings : MonoBehaviour
    {
        static LocalizationSettings _instance;
        static LocalizationSettings instance
        {
            get
            {
                if (!_instance)
                    _instance = FindAnyObjectByType<LocalizationSettings>();
                return _instance;
            }
        }

        int languageId
        {
            get
            {
                var key = "LOCALIZATION_KEY_LANGUAGE_ID";
                var value = PlayerPrefs.GetInt(key, 0);
                return value;
            }
            set
            {
                var key = "LOCALIZATION_KEY_LANGUAGE_ID";
                PlayerPrefs.SetInt(key, value);
            }
        }
        
        public static int GetLanguageId()
        {
            if (!instance)
            {
                LocalizationModuleDebug.LogError("LocalizationSettings is not initialized.");
                return -1;
            }
            
            return instance.languageId;
        }
        
        public static bool SetLanguage(int id)
        {
            if (!instance)
            {
                LocalizationModuleDebug.LogError("LocalizationSettings is not initialized.");
                return false;
            }
            
            instance.languageId = id;
            LocalizationManager.InvokeListenersOnLanguageChanged();
            
            return true;
        }
    }
}