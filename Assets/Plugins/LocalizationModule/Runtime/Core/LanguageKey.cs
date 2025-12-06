using System;
using Unity.Collections;
using UnityEngine;

namespace LocalizationModule.Runtime
{
    [Serializable]
    public class LanguageKey
    {
        public LanguageKey(int id, string name, Sprite sprite)
        {
            this.id = id;
            
            this.name = name;
            this.icon = sprite;
        }
        
        [ReadOnly] public int id;
        
        [Space(5)]
        public string name;
        public Sprite icon;
    }
}