using System.IO;
using System.Linq;
using GameDevUtils.Editor;
using LocalizationModule.Runtime;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace LocalizationModule.Editor
{
    internal static class LocalizationEditorUtils
    {
        static string packageBasePath;
        public static string PACKAGE_BASE_PATH
        {
            get
            {
                if (!string.IsNullOrEmpty(packageBasePath))
                    return packageBasePath;
                
                packageBasePath = EditorHelper.GetBasePathOfScript("LocalizationEditorUtils");
                return packageBasePath;
            }
        }
        
        internal static string PACKAGE_EDITOR_PATH => PACKAGE_BASE_PATH + "/Editor";

        internal static string ASSETS_PATH => "Assets/Isolarv Data/Localization Module";
        
        internal static string KEYS_PATH => ASSETS_PATH + "/Keys";
        internal static string TABLES_PATH => ASSETS_PATH + "/Tables";
        
        private static Texture _toolIcon;
        public static GUIContent GetWindowTitle(string windowName)
        {
            if(!_toolIcon)
                _toolIcon = AssetDatabase.LoadAssetAtPath<Texture>($"{PACKAGE_BASE_PATH}/Sprites/icon.png");
            return new GUIContent(windowName, _toolIcon);
        }

        public static T[] LoadAllAssetsInFolder<T>(string folder) where T : Object
        {
            string directoryPath = Path.GetDirectoryName(folder + "/");
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
            
            string[] guids = AssetDatabase.FindAssets("t:" + typeof(T).Name, new[] { folder });
            T[] assets = new T[guids.Length];
            
            for (int i = 0; i < guids.Length; i++)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);
                assets[i] = AssetDatabase.LoadAssetAtPath<T>(assetPath);
            }
            
            return assets;
        }
        
        public static void ValidateTableOfKeys(LocalizationKeyCollection collection)
        {
            var tableName = collection.name.Replace("_KeyCollection", "_TranslateTable");
            var folder = $"{TABLES_PATH}";
            var path = folder + $"/{tableName}.asset";

            var asset = AssetDatabase.LoadAssetAtPath<TranslateTable>(path);
            if (!asset)
            {
                asset = ScriptableObject.CreateInstance<TranslateTable>();
                CreateAsset(asset, path);
                EditorUtility.SetDirty(asset);
                
                collection.Table = asset;
                EditorUtility.SetDirty(collection);
            }

            collection.Table.SetRelatedKeys(collection);
            if (collection.Table != asset)
            {
                collection.Table = asset;
                EditorUtility.SetDirty(collection);
            }
            
            collection.OnTableValidate(tableName);
        }

        public static void CreateAsset(Object asset, string assetPath)
        {
            string directoryPath = Path.GetDirectoryName(assetPath);
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
            
            AssetDatabase.CreateAsset(asset, assetPath);
        }
    }
}