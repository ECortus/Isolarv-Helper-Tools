using TMPro;
using UnityEngine;

namespace LocalizationModule.Runtime.Components
{
    [RequireComponent(typeof(TMP_Text))]
    public class LocalizeText : BaseLocalizeComponent
    {
        TMP_Text _text;
        
        protected override LocalizationKey.KeyType keyType => LocalizationKey.KeyType.Text;

        protected override void Localize()
        {
            _text = GetComponent<TMP_Text>();
            if (!_text)
            {
                LocalizationModuleDebug.LogError($"Invalid Text on {gameObject.name} to localize!");
                return;
            }
            
            _text.text = GetLocalizedObject();
        }
        
        string GetLocalizedObject()
        {
            return LocalizationManager.GetTranslationText(key);
        }
    }
}