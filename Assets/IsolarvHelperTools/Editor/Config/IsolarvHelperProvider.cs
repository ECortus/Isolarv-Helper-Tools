using System.Collections.Generic;
using UnityEditor;

namespace IsolarvHelperTools.Editor
{
    public static class IsolarvHelperProvider
    {
        [SettingsProvider]
        public static SettingsProvider CreateSettingsProvider()
        {
            var provider = new SettingsProvider("Preferences/Isolarv Helper Tools", SettingsScope.User)
            {
                label = "Isolarv Helper Tools",

                guiHandler = (searchContext) =>
                {
                    InspectorEditorHelper.Space(10);
                    EditorGUI.BeginChangeCheck();
                    
                    var settings = IsolarvHelperConfigHandler.GetEditorSettings();
                    DrawDebugMenu(ref settings);

                    if (EditorGUI.EndChangeCheck())
                    {
                        IsolarvHelperConfigHandler.SetEditorSettings(settings);
                    }
                },

                keywords = new HashSet<string>(new[] { "Isolarv", "Helper", "Tools" })
            };

            return provider;
        }

        static void DrawDebugMenu(ref IsolarvHelperConfig settings)
        {
            settings.enableProjectDebug = EditorGUILayout.Toggle("Enable Project Debug", settings.enableProjectDebug);

            if (settings.enableProjectDebug)
            {
                InspectorEditorHelper.DrawHeader("Logging");
                settings.enableLogging = EditorGUILayout.Toggle("Enable Logging", settings.enableLogging);
            }
        }
    }
}