using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace GameSaveKit.Editor
{
    internal static class GameSaveKitEditorUtils
    {
        static string packageBasePath;
        public static string PACKAGE_BASE_PATH
        {
            get
            {
                if (!string.IsNullOrEmpty(packageBasePath))
                    return packageBasePath;

                var dataPath = Application.dataPath
                    .Replace("/Assets", "");

                string[] res = System.IO.Directory.GetFiles(dataPath, "GameSaveKitEditorUtils.cs", SearchOption.AllDirectories);
                
                if (res.Length == 0)
                    throw new Exception("GameSaveKitEditorUtils.cs not found");

                //TODO: adapt script path to work with Assets
                var scriptPath = res[0].Replace(dataPath, "")
                    .Replace("\\", "/")
                    .Replace("GameSaveKitEditorUtils.cs", "")
                    .Replace("/Editor/", "")
                    .Remove(0, 1);
                
                //TODO: adapt script path to work with Packages
                scriptPath = scriptPath.Replace("Library/PackageCache", "Packages");
                
                var indexOfAtSign = scriptPath.IndexOf("@", StringComparison.Ordinal);
                if (indexOfAtSign != -1)
                {
                    scriptPath = scriptPath.Remove(indexOfAtSign, 13);
                }
                
                Debug.Log(scriptPath);

                packageBasePath = scriptPath;
                return packageBasePath;
            }
        }
        
        public static string PACKAGE_EDITOR_PATH => $"{PACKAGE_BASE_PATH}/Editor";
        
        private static Texture _toolIcon;
        public static GUIContent GetWindowTitle(string windowName)
        {
            if(!_toolIcon)
                _toolIcon = AssetDatabase.LoadAssetAtPath<Texture>($"{PACKAGE_BASE_PATH}/Sprites/icon.png");
            return new GUIContent(windowName, _toolIcon);
        }

        public static T[] LoadAllAssetsInFolder<T>(string folder) where T : Object
        {
            string[] guids = AssetDatabase.FindAssets("t:" + typeof(T).Name, new[] { folder });
            T[] assets = new T[guids.Length];
            
            for (int i = 0; i < guids.Length; i++)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);
                assets[i] = AssetDatabase.LoadAssetAtPath<T>(assetPath);
            }
            
            return assets;
        }

        public static void CreateAsset(Object asset, string assetPath)
        {
            string directoryPath = Path.GetDirectoryName(assetPath);
            if (!Directory.Exists(directoryPath))
            {
                if (directoryPath == null)
                    throw new ArgumentNullException(nameof(directoryPath));
                
                Directory.CreateDirectory(directoryPath);
            }
            
            AssetDatabase.CreateAsset(asset, assetPath);
        }
    }
}