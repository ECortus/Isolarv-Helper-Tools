using System;
using UnityEngine;

namespace LocalizationModule.Runtime
{
    [Serializable]
    public class LocalizationKey
    {
        public LocalizationKey(string key, KeyType type)
        {
            defaultKey = key;
            this.type = type;
        }
        
        public enum KeyType
        {
            Text, Sprite, Texture
        }
            
        [SerializeField] private KeyType type;
        [SerializeField] private string defaultKey;

        public string key
        {
            get
            {
                string newKey = defaultKey;
                if (type == KeyType.Text)
                    newKey += "_TEXT";
                else if (type == KeyType.Sprite)
                    newKey += "_SPRITE";
                else if (type == KeyType.Texture)
                    newKey += "_TEXTURE";
                else
                    throw new NotImplementedException();
                    
                return newKey;
            }
        }
            
#if UNITY_EDITOR
            
        public KeyType Type => type;
            
#endif
    }
}