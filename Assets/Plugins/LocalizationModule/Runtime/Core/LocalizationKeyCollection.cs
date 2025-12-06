using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using UnityEditor;
using UnityEngine;

namespace LocalizationModule.Runtime
{
    public class LocalizationKeyCollection : ScriptableObject 
    {
        [SerializeField] List<LocalizationKey> keys = new List<LocalizationKey>();
        [SerializeField] TranslateTable relatedTable;

        public List<LocalizationKey> GetKeysInfo() => keys;
        
        public List<string> GetKeys()
        {
            return GetKeys(keys);
        }
        
        public List<string> GetKeys(List<LocalizationKey> other)
        {
            List<string> list = new List<string>();
            foreach (LocalizationKey key in other)
            {
                list.Add(key.key);
            }
            
            return list;
        }

#if UNITY_EDITOR

        public void OnTableValidate(string newTableName)
        {
            LocalizationModuleDebug.Log($"Localization keys {name} is validated and related to table {newTableName}.");
        }

        public TranslateTable Table
        {
            get => relatedTable;
            set
            {
                relatedTable = value;
                EditorUtility.SetDirty(this);
            }
        }
        
        public LocalizationKey GetKey(string key)
        {
            for (int i = 0; i < keys.Count; i++)
            {
                if (keys[i].key == key)
                {
                    return keys[i];
                }
            }
            
            LocalizationModuleDebug.LogError($"Localization key with id {key} not found.");
            return null;
        }

        public int GetIndexOfKey(string key)
        {
            for (int i = 0; i < keys.Count; i++)
            {
                if (keys[i].key == key)
                {
                    return i;
                }
            }
            
            LocalizationModuleDebug.LogError($"Localization key with id {key} not found.");
            return -1;
        }

        public int keysCount => keys.Count;
        
        public void AddKey(LocalizationKey key)
        {
            keys.Add(key);
            EditorUtility.SetDirty(this);
        }
        
        public void RemoveKey(int index)
        {
            keys.RemoveAt(index);
            EditorUtility.SetDirty(this);
        }
        
        public bool ScanForDuplicate()
        {
            HashSet<string> unique = new HashSet<string>(GetKeys());
            if (unique.Count != keys.Count)
            {
                return true;
            }

            return false;
        }
        
        public void RemoveDuplicates()
        {
            HashSet<string> unique = new HashSet<string>(GetKeys());
            for (int i = 0; i < keys.Count; i++)
            {
                if (!unique.Contains(keys[i].key))
                {
                    keys.RemoveAt(i);
                    i--;
                }
                else
                {
                    unique.Remove(keys[i].key);
                }
            }
            
            LocalizationModuleDebug.LogWarning($"Duplicate keys in {name} are removed.");
            EditorUtility.SetDirty(this);
        }
#endif
    }
}