using Cysharp.Threading.Tasks;
using LocalizationModule.Runtime;
using UnityEditor;
using UnityEngine;

namespace LocalizationModule.Editor
{
    internal static class LocalizationToolsCommand
    {
        [MenuItem("Tools/Isolarv/Localization Module/Validate all tables", false, 2)]
        public static async void ValidateAllTables()
        {
            await ValidateByTables();
            await ValidateByKeys();
        }
        
        [MenuItem("Tools/Isolarv/Localization Module/All windows", false, 15)]
        public static void ShowAllWindows()
        {
            ShowLanguageWindow();
            
            LocalizationKeysWindow.OpenWindow(typeof(LanguageKeysWindow));
            TranslateTablesWindow.OpenWindow(typeof(LanguageKeysWindow));
            
            EditorWindowUtils.FocusWindow<LanguageKeysWindow>("Languages");
        }
        
        [MenuItem("Tools/Isolarv/Localization Module/Languages", false, 16)]
        public static void ShowLanguageWindow()
        {
            EditorWindowUtils.ShowWindow<LanguageKeysWindow>("Languages");
        }
        
        [MenuItem("Tools/Isolarv/Localization Module/Keys", false, 16)]
        public static void ShowKeysWindow()
        {
            EditorWindowUtils.ShowWindow<LocalizationKeysWindow>("Keys");
        }
        
        [MenuItem("Tools/Isolarv/Localization Module/Tables", false, 16)]
        public static void ShowTablesWindow()
        {
            EditorWindowUtils.ShowWindow<TranslateTablesWindow>("Tables");
        }

        static async UniTask ValidateByKeys()
        {
            var keys = AssetDatabase.FindAssets("t:LocalizationKeyCollection", new string[] 
                { $"{EditorUtils.KEYS_PATH}" });
            
            foreach (var key in keys)
            {
                var path = AssetDatabase.GUIDToAssetPath(key);
                var asset = AssetDatabase.LoadAssetAtPath<LocalizationKeyCollection>(path);
                
                EditorUtils.ValidateTableOfKeys(asset);
                
                await UniTask.Yield();
            }
        }

        static async UniTask ValidateByTables()
        {
            var tables = AssetDatabase.FindAssets("t:TranslateTable", new string[] 
                { $"{EditorUtils.TABLES_PATH}" });
            
            foreach (var table in tables)
            {
                var path = AssetDatabase.GUIDToAssetPath(table);
                var asset = AssetDatabase.LoadAssetAtPath<TranslateTable>(path);
                
                if (!asset.IsValidatedTable())
                {
                    AssetDatabase.DeleteAsset(path);
                }
                else
                {
                    EditorWindowUtils.ValidateTable(asset);
                    asset.OnTableValidate();
                }
                
                await UniTask.Yield();
            }
        }
    }
}