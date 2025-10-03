using System;
using UnityEngine;

namespace IsolarvHelperTools
{
    [Serializable]
    public class IsolarvDebugConfig
    {
        static IsolarvDebugConfig _i = null;
        static IsolarvDebugConfig Instance
        {
            get
            {
                if (_i == null)
                    _i = IsolarvDebugConfigHandler.GetDebugSettings();
                return _i;
            }
        }
        
        #region Logging

        public bool enableLogging = false;
        public bool instantiateLoggerInProject = false;
        
        public static bool ENABLE_MANUAL_LOGGING => Instance.enableLogging;

        #endregion
    }
    
    public class IsolarvDebugConfigHandler
    {
        private const string CONFIG_NAME = "ISOLARV_DEBUG_CONFIG_SAVE_FILE";
        
        public static IsolarvDebugConfig GetDebugSettings()
        {
            var str = PlayerPrefs.GetString(CONFIG_NAME, "{}");
            var config = JsonUtility.FromJson<IsolarvDebugConfig>(str);
            return config;
        }

        public static void SetDebugSettings(IsolarvDebugConfig settings)
        {
            var config = JsonUtility.ToJson(settings);
            PlayerPrefs.SetString(CONFIG_NAME, config);
        }
    }
}