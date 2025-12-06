using Cysharp.Threading.Tasks;
using LocalizationModule.Runtime;
using UnityEditor;
using UnityEngine;

namespace LocalizationModule.Editor
{
    internal static class LocalizationToolsCommand
    {
        [MenuItem("Tools/Isolarv/Localization Tool/Validate all tables", false, 115)]
        public static async void ValidateAllTables()
        {
            await ValidateByTables();
            await ValidateByKeys();
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