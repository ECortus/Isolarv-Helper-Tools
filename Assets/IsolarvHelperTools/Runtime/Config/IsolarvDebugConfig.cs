using System;
using UnityEngine;

namespace IsolarvHelperTools.Runtime
{
    [Serializable]
    public class IsolarvDebugConfig
    {
        public bool ENABLE_DEBUG_LOGGING = true;

        public delegate void ConfigChangeMethod(ref IsolarvDebugConfig config);

        public static void SetChanges(ConfigChangeMethod callback)
        {
            var config = GetConfig();

            callback(ref config);
            SaveConfig(config);
        }

        #region Get & Save Instance

        static IsolarvDebugConfig _i = null;
        public static IsolarvDebugConfig GetConfig()
        {
            if (_i == null)
                _i = Handler.GetDebugSettings();

            if (_i == null)
                throw new SystemException("[DEBUG CONFIG] IsolarvDebugConfig instance is null!");

            return _i;
        }

        static void SaveConfig(IsolarvDebugConfig settings)
        {
            Handler.SetDebugSettings(settings);
        }

        #endregion

        static class Handler
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
}