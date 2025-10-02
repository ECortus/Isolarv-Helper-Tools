using UnityEngine;

namespace IsolarvHelperTools
{
    public static class DebugHelper
    {
        public static void Log(string message)
        {
            if (!IsolarvHelperConfig.ENABLE_LOGGING)
                return;
            
            Debug.Log(message);
        }

        public static void LogWarning(string message)
        {
            if (!IsolarvHelperConfig.ENABLE_LOGGING)
                return;
            
            Debug.LogWarning(message);
        }

        public static void LogError(string message)
        {
            if (!IsolarvHelperConfig.ENABLE_LOGGING)
                return;
            
            Debug.LogError(message);
        }
    }
}