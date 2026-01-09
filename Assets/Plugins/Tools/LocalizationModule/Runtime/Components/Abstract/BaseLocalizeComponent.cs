using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace LocalizationModule.Runtime.Components
{
    public abstract class BaseLocalizeComponent : MonoBehaviour
    {
        [SerializeField] private LocalizationKeyCollection localizationKeys;
        [SerializeField] private int keyIndex = -1;

        protected string key => localizationKeys.GetKeysInfo()[keyIndex].key;

        void Awake()
        {
            LocalizationManager.AddListenerOnInitialize(Initialize);
        }

        void Initialize()
        {
            if (!localizationKeys)
            {
                LocalizationModuleDebug.LogError($"Invalid LocalizationKeys on {gameObject.name} to localize!");
                return;
            }

            if (keyIndex == -1)
            {
                LocalizationModuleDebug.LogError($"Invalid key index on {gameObject.name} to localize!");
                return;
            }
                
            Localize();
            LocalizationManager.AddListenerOnLanguageChanged(Localize);
        }

        protected abstract LocalizationKey.KeyType keyType { get; }

        protected abstract void Localize();

#if UNITY_EDITOR
        public LocalizationKey.KeyType EDITOR_KeyType => keyType;
#endif
    }
}