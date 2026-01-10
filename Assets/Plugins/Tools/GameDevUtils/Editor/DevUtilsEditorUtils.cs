using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace GameDevUtils.Editor
{
    internal static class DevUtilsEditorUtils
    {
        static string packageBasePath;
        static string PACKAGE_BASE_PATH
        {
            get
            {
                if (!string.IsNullOrEmpty(packageBasePath))
                    return packageBasePath;
                
                packageBasePath = EditorHelper.GetBasePathOfScript("DevUtilsEditorUtils");
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