using UnityEngine;

namespace IsolarvHelperTools
{
    public static class DebugHelper
    {
        public static void Log(string message)
        {
            if (!IsolarvDebugConfig.ENABLE_MANUAL_LOGGING)
                return;
            
            Debug.Log(message);
        }

        public static void LogWarning(string message)
        {
            if (!IsolarvDebugConfig.ENABLE_MANUAL_LOGGING)
                return;
            
            Debug.LogWarning(message);
        }

        public static void LogError(string message)
        {
            if (!IsolarvDebugConfig.ENABLE_MANUAL_LOGGING)
                return;
            
            Debug.LogError(message);
        }
    }
}