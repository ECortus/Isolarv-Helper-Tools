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
        
        public bool enableProjectDebug = false;

        #region Logging

        public bool enableLogging = false;
        public bool instantiateLoggerInProject = false;
        
        public static bool ENABLE_MANUAL_LOGGING => Instance.enableProjectDebug && Instance.enableLogging;
        public static bool INSTANTIATE_LOGGER_IN_PROJECT => Instance.enableProjectDebug && Instance.instantiateLoggerInProject;

        #endregion
    }
    
    public class IsolarvDebugConfigHandler
    {
        private const string CONFIG_NAME = "Isolarv-Helper-Config-Debug";
        
        public static IsolarvDebugConfig GetDebugSettings()
        {
            var config = JsonUtility.FromJson<IsolarvDebugConfig>(PlayerPrefs.GetString(CONFIG_NAME, "{}"));
            return config;
        }

        public static void SetDebugSettings(IsolarvDebugConfig settings)
        {
            var config = JsonUtility.ToJson(settings);
            PlayerPrefs.SetString(CONFIG_NAME, config);
        }
    }
}