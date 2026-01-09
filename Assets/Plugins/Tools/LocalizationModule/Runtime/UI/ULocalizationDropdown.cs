using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LocalizationModule.Runtime.UI
{
    public class ULocalizationDropdown : MonoBehaviour
    {
        [SerializeField] private TMP_Dropdown dropdown;
        
        LanguageKeyCollection _languageKeys;

        void Awake()
        {
            LocalizationManager.AddListenerOnInitialize(Initialize);
        }

        void Initialize()
        {
            var options = new List<TMP_Dropdown.OptionData>();

            _languageKeys = LocalizationManager.GetLanguages();
            var keys = _languageKeys.GetKeys();
            
            foreach (LanguageKey key in keys)
            {
                options.Add(GetOptionData(key));
            }
            
            dropdown.options = options;

            dropdown.value = LocalizationSettings.GetLanguageId();
            dropdown.onValueChanged.AddListener(OnValueChanged);
        }

        void OnValueChanged(int i)
        {
            var keys = _languageKeys.GetKeys();
            var key = keys[i];

            var id = key.id;
            LocalizationSettings.SetLanguage(id);
        }

        TMP_Dropdown.OptionData GetOptionData(LanguageKey key)
        {
            var data = new TMP_Dropdown.OptionData(key.name);
            data.image = key.icon;
            
            return data;
        }
    }
}