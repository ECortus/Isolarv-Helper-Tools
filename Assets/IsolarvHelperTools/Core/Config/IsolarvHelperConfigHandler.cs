using UnityEngine;

namespace IsolarvHelperTools
{
    public class IsolarvHelperConfigHandler
    {
        private const string CONFIG_NAME = "Isolarv-Helper-Config-Debug";
        
        public static IsolarvHelperConfig GetEditorSettings()
        {
            var config = JsonUtility.FromJson<IsolarvHelperConfig>(PlayerPrefs.GetString(CONFIG_NAME, "{}"));
            return config;
        }

        public static void SetEditorSettings(IsolarvHelperConfig settings)
        {
            var config = JsonUtility.ToJson(settings);
            PlayerPrefs.SetString(CONFIG_NAME, config);
        }
    }
}