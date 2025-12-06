using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LocalizationModule.Runtime
{
    [Serializable]
    public class TranslateInfo
    {
        [Serializable]
        public abstract class TranslateInfoBase
        {
            public int languageId;

            public TranslateInfoBase()
            {
                
            }
            
            public TranslateInfoBase(int languageId)
            {
                this.languageId = languageId;
            }
        }
        
        [Serializable]
        public class TranslateInfoText : TranslateInfoBase
        {
            public string text;
            
            public TranslateInfoText(int languageId, string text)
            {
                this.languageId = languageId;
                this.text = text;
            }
        }
        
        [Serializable]
        public class TranslateInfoSprite : TranslateInfoBase
        {
            public Sprite sprite;
            
            public TranslateInfoSprite(int languageId, Sprite sprite)
            {
                this.languageId = languageId;
                this.sprite = sprite;
            }
        }
        
        [Serializable]
        public class TranslateInfoTexture : TranslateInfoBase
        {
            public Texture texture;
            
            public TranslateInfoTexture(int languageId, Texture texture)
            {
                this.languageId = languageId;
                this.texture = texture;
            }
        }
        
        public List<TranslateInfoText> text = new List<TranslateInfoText>();
        public List<TranslateInfoSprite> sprite = new List<TranslateInfoSprite>();
        public List<TranslateInfoTexture> texture = new List<TranslateInfoTexture>();
        
        public string GetText(int languageId)
        {
            foreach (var info in text)
            {
                if (info.languageId == languageId)
                    return info.text;
            }
            
            LocalizationModuleDebug.LogError($"Translation info by language id {languageId} not found.");
            return "";
        }
        
        public Sprite GetSprite(int languageId)
        {
            foreach (var info in sprite)
            {
                if (info.languageId == languageId)
                    return info.sprite;
            }
            
            LocalizationModuleDebug.LogError($"Translation info by language id {languageId} not found.");
            return null;
        }
        
        public Texture GetTexture(int languageId)
        {
            foreach (var info in texture)
            {
                if (info.languageId == languageId)
                    return info.texture;
            }
            
            LocalizationModuleDebug.LogError($"Translation info by language id {languageId} not found.");
            return null;
        }
    }
}