using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace LocalizationModule.Runtime
{
    internal class LocalizationManager : MonoBehaviour
    {
        static LocalizationManager _instance;
        static LocalizationManager instance
        {
            get
            {
                if (!_instance)
                    _instance = FindAnyObjectByType<LocalizationManager>();
                return _instance;
            }
        }
        
        [SerializeField] private AssetReference languagesReference;
        [SerializeField] private AssetReference[] tablesReference;
        
        LanguageKeyCollection _languages;
        TranslateTable[] _tables;

        bool _initialized = false;

        #region Initialize

        void Awake()
        {
            LocalizationAddressableInitializer.AddListener(Initialize);
        }

        void Initialize(AsyncOperationHandle<IResourceLocator> locator)
        {
            LocalizationModuleDebug.Log("LocalizationManager load assets...");
            
            UniTask.Create(async () =>
            {
                await LoadLanguageAsync();
                await LoadTablesAsync();

                InvokeListenersOnInitialize();
            });
            
            LocalizationModuleDebug.Log("LocalizationManager successfully loaded assets and initialized.");
            
            DontDestroyOnLoad(this.gameObject);
        }

        async UniTask LoadLanguageAsync()
        {
            var task = languagesReference.LoadAssetAsync<LanguageKeyCollection>();
            task.Completed += (asyncOperation) =>
            {
                if (asyncOperation.Status == AsyncOperationStatus.Succeeded)
                {
                    _languages = asyncOperation.Result;
                }
                else
                {
                    LocalizationModuleDebug.LogError("Failed to load languages.");
                }
            };

            await task;
        }
        
        async UniTask LoadTablesAsync()
        {
            var length = tablesReference.Length;
            _tables = new TranslateTable[length];

            List<AsyncOperationHandle<TranslateTable>> tasks = new List<AsyncOperationHandle<TranslateTable>>();
            
            for (int i = 0; i < length; i++)
            {
                var tableReference = tablesReference[i];
                var index = i;

                var task = tableReference.LoadAssetAsync<TranslateTable>();
                task.Completed += (asyncOperation) =>
                {
                    if (asyncOperation.Status == AsyncOperationStatus.Succeeded)
                    {
                        _tables[index] = asyncOperation.Result;
                    }
                    else
                    {
                        LocalizationModuleDebug.LogError($"Failed to load translation table {tableReference.Asset.name}.");
                    }
                };
                
                tasks.Add(task);
            }
            
            for (int i = 0; i < length; i++)
            {
                await tasks[i];
            }
        }

        #endregion

        #region Get-Methods

        public static LanguageKeyCollection GetLanguages()
        {
            if (!instance)
            {
                LocalizationModuleDebug.LogError("LocalizationManager is not initialized.");
                return null;
            }
            
            return instance._languages;
        }
    
        public static string GetTranslationText(string key)
        {
            if (!instance)
            {
                LocalizationModuleDebug.LogError("LocalizationManager is not initialized.");
                return "";
            }
            
            var languageId = LocalizationSettings.GetLanguageId();
            foreach (var table in instance._tables)
            {
                if (table.TryGetTranslateInfo(key, out TranslateInfo info))
                {
                    return info.GetText(languageId);
                }
            }
            
            LocalizationModuleDebug.LogError($"Translation key {key} not found.");
            return "";
        }
        
        public static Sprite GetTranslationSprite(string key)
        {
            if (!instance)
            {
                LocalizationModuleDebug.LogError("LocalizationManager is not initialized.");
                return null;
            }
            
            var languageId = LocalizationSettings.GetLanguageId();
            foreach (var table in instance._tables)
            {
                if (table.TryGetTranslateInfo(key, out TranslateInfo info))
                {
                    return info.GetSprite(languageId);
                }
            }
            
            LocalizationModuleDebug.LogError($"Translation key {key} not found.");
            return null;
        }
        
        public static Texture GetTranslationTexture(string key)
        {
            if (!instance)
            {
                LocalizationModuleDebug.LogError("LocalizationManager is not initialized.");
                return null;
            }
            
            var languageId = LocalizationSettings.GetLanguageId();
            foreach (var table in instance._tables)
            {
                if (table.TryGetTranslateInfo(key, out TranslateInfo info))
                {
                    return info.GetTexture(languageId);
                }
            }
            
            LocalizationModuleDebug.LogError($"Translation key {key} not found.");
            return null;
        }

        #endregion

        #region Events

        event Action OnInitialize;
        
        public static void AddListenerOnInitialize(Action listener)
        {
            if (!instance)
            {
                LocalizationModuleDebug.LogError("LocalizationManager is not initialized.");
                return;
            }

            if (_instance._initialized)
            {
                listener();
                return;
            }
            
            instance.OnInitialize += listener;
        }

        void InvokeListenersOnInitialize()
        {
            _initialized = true;
            OnInitialize?.Invoke();
        }
        
        event Action OnLanguageChanged;
        
        public static void AddListenerOnLanguageChanged(Action listener)
        {
            if (!instance)
            {
                LocalizationModuleDebug.LogError("LocalizationManager is not initialized.");
                return;
            }
            
            instance.OnLanguageChanged += listener;
        }

        public static void InvokeListenersOnLanguageChanged()
        {
            if (!instance)
            {
                LocalizationModuleDebug.LogError("LocalizationManager is not initialized.");
                return;
            }
            
            instance.OnLanguageChanged?.Invoke();
        }

        #endregion
    }
}