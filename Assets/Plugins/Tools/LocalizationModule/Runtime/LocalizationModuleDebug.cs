using UnityEngine;

namespace LocalizationModule.Runtime
{
    internal static class LocalizationModuleDebug
    {
        public static void Log(string message)
        {
            if (PackageSettings.ShowDebugLogs)
            {
                Debug.Log("[Localization Tool] " + message);
            }
        }

        public static void LogWarning(string message)
        {
            if (PackageSettings.ShowDebugLogs)
            {
                Debug.LogWarning("[Localization Tool] " + message);
            }
        }

        public static void LogError(string message)
        {
            if (PackageSettings.ShowDebugLogs)
            {
                Debug.LogError("[Localization Tool] " + message);
            }
        }
    }
}