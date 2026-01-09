using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace LocalizationModule.Runtime
{
    public class TranslateTable : ScriptableObject
    {
        [SerializeField] private LocalizationKeyCollection relatedKeys;

        [Serializable]
        public class TranslationKey
        {
            [SerializeField] string key;
            [SerializeField] TranslateInfo translateInfo;
            
            public TranslationKey(string key, TranslateInfo translateInfo)
            {
                this.key = key;
                this.translateInfo = translateInfo;
            }

            public bool TryGetValue(string compareKey, out TranslateInfo info)
            {
                if (key == compareKey)
                {
                    info = translateInfo;
                    return true;
                }

                info = null;
                return false;
            }

#if UNITY_EDITOR
            public string GetKey() => key;
            public TranslateInfo GetTranslateInfo() => translateInfo;
#endif
        }
        
        [SerializeField] List<TranslationKey> _translation = new List<TranslationKey>();

        public bool TryGetTranslateInfo(string key, out TranslateInfo info)
        {
            foreach (var translationKey in _translation)
            {
                if (translationKey.TryGetValue(key, out info))
                {
                    return true;
                }
            }

            info = null;
            return false;
        }

#if UNITY_EDITOR

        public List<TranslationKey> translation
        {
            get => _translation;
            set => _translation = value;
        }
        
        public void SetRelatedKeys(LocalizationKeyCollection keys)
        {
            relatedKeys = keys;
        }
        
        public LocalizationKeyCollection GetRelatedKeys() => relatedKeys;
        
        public bool IsValidatedTable()
        {
            return relatedKeys != null;
        }
        
        public void OnTableValidate()
        {
            LocalizationModuleDebug.Log($"Table {AssetDatabase.GetAssetPath(this)} is validated.");
        }
#endif
    }
}