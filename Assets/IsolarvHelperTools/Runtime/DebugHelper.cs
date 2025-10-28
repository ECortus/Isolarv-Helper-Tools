using UnityEngine;

namespace IsolarvHelperTools.Runtime
{
    public static class DebugHelper
    {
        static IsolarvDebugConfig _i = null;
        static IsolarvDebugConfig config
        {
            get
            {
                if (_i == null)
                _i = IsolarvDebugConfig.GetConfig();
                return _i;
            }
        }

        public static void Log(string message)
        {
            if (!config.ENABLE_MANUAL_LOGGING)
                return;
            
            Debug.Log("[ISOLARV DEBUG ]" + message);
        }

        public static void LogWarning(string message)
        {
            if (!config.ENABLE_MANUAL_LOGGING)
                return;
            
            Debug.LogWarning("[ISOLARV DEBUG ]" + message);
        }

        public static void LogError(string message)
        {
            if (!config.ENABLE_MANUAL_LOGGING)
                return;
            
            Debug.LogError("[ISOLARV DEBUG ]" + message);
        }
    }
}