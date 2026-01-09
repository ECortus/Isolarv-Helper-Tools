using System.Linq;
using LocalizationModule.Runtime;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace LocalizationModule.Editor
{
    internal class AddNewLanguage : VisualElement
    {
        IntegerField _idField;
        TextField _nameField;
        ObjectField _spriteField;
        
        LanguageKeyCollection _languageKeyCollection;
        LanguageKeysWindow _mainWindow;
        
        public AddNewLanguage(LanguageKeyCollection languageKeyCollection, LanguageKeysWindow mainWindow)
        {
            _mainWindow = mainWindow;
            _languageKeyCollection = languageKeyCollection;
            
            var visualTree =
                AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(
                    $"{LocalizationEditorUtils.PACKAGE_EDITOR_PATH}/Editor Window/Language Window/AddNewLanguageWindow.uxml");
            visualTree.CloneTree(this);
            
            var contentBox = this.Q<GroupBox>("fields-box");
            
            _idField = new IntegerField("ID");
            _nameField = new TextField("Name");
            _spriteField = new ObjectField("Sprite");
            _spriteField.objectType = typeof(Sprite);
            
            contentBox.Add(_idField);
            contentBox.Add(_nameField);
            contentBox.Add(_spriteField);
            
            var addButton = this.Q<Button>("add-language-button");
            addButton.clicked += TryCreateNewLanguage;
        }

        void TryCreateNewLanguage()
        {
            bool canAdd = true;

            ResetColors();
            
            var id = _idField.value;
            if (id < 0 || _languageKeyCollection.GetKeys().Any(x => x.id == id))
            {
                var color = Color.red;
                color.a = 0.5f;
                
                _idField.style.backgroundColor = color;
                canAdd = false;
            }

            var keyName = _nameField.value;
            if (_nameField.text == "" || _languageKeyCollection.GetKeys().Any(x => x.name == keyName))
            {
                var color = Color.red;
                color.a = 0.5f;
                
                _nameField.style.backgroundColor = color;
                canAdd = false;
            }
            
            var sprite = _spriteField.value as Sprite;
            if (!sprite)
            {
                var color = Color.red;
                color.a = 0.5f;
                
                _spriteField.style.backgroundColor = color;
                canAdd = false;
            }

            if (!canAdd)
            {
                return;
            }
            
            var languageKey = new LanguageKey(id, keyName, sprite);
            _languageKeyCollection.AddNewKey(languageKey);

            ResetBox();
            
            _mainWindow.UpdateKeysSet();

            int index = _languageKeyCollection.keysCount - 1;
            _mainWindow.OnLanguageSelected(index);
        }

        void ResetBox()
        {
            ResetColors();
            
            _idField.value = 0;
            _nameField.value = "";
            _spriteField.value = null;
        }

        void ResetColors()
        {
            _idField.style.backgroundColor = Color.clear;
            _nameField.style.backgroundColor = Color.clear;
            _spriteField.style.backgroundColor = Color.clear;
        }
    }
}