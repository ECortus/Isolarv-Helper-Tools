using UnityEngine;

namespace GameSaveKit.Runtime.Settings
{
    [System.Serializable]
    public sealed class SaveKitPathSettings
    {
        public static string GetDirectory()
        {
            var data = Application.persistentDataPath;
            var userId = "000000000";
            
            return $"{data}/{userId}";
        }
        
        public static string GetOrCreateFilePath(string fileName, string fileExtension)
        {
            var directory = GetDirectory();
            return $"{directory}/{fileName}.{fileExtension}";
        }
    }
}