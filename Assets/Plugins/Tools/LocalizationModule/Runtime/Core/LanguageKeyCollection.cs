using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace LocalizationModule.Runtime
{
    public class LanguageKeyCollection : ScriptableObject
    {
        [SerializeField] private List<LanguageKey> keys = new List<LanguageKey>();
        public List<LanguageKey> GetKeys() => keys;

#if UNITY_EDITOR
        
        public int keysCount => keys.Count;
        
        public void AddNewKey(LanguageKey key)
        {
            keys.Add(key);
            EditorUtility.SetDirty(this);
        }
        
        public void RemoveKey(int index)
        {
            keys.RemoveAt(index);
            EditorUtility.SetDirty(this);
        }

        public LanguageKey GetKeyByID(int id)
        {
            for (int i = 0; i < keys.Count; i++)
            {
                if (keys[i].id == id)
                {
                    return keys[i];
                }
            }
            
            LocalizationModuleDebug.LogError($"Language key with id {id} not found.");
            return null;
        }

        public bool ContainKeyById(int id)
        {
            for (int i = 0; i < keys.Count; i++)
            {
                if (keys[i].id == id)
                {
                    return true;
                }
            }
            
            return false;
        }
#endif
    }
}