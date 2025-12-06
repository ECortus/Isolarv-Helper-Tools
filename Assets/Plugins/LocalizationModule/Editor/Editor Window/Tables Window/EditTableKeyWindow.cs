using LocalizationModule.Runtime;
using UnityEditor;
using UnityEditor.Search;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using ObjectField = UnityEditor.UIElements.ObjectField;

namespace LocalizationModule.Editor
{
    internal class EditTableKeyWindow : VisualElement
    {
        LanguageKeyCollection _languageKeyCollection;
        LocalizationKey _relatedKey;
        TranslateTable _table;
        TranslateInfo _info;
        
        int _mainIndex;
        
        TranslateTablesWindow _mainWindow;
        
        Label _keyLabel;
        
        public EditTableKeyWindow(TranslateTable table, string key, int mainIndex, TranslateTablesWindow mainWindow)
        {
            _mainWindow = mainWindow;
            _mainIndex = mainIndex;
            _table = table;

            _languageKeyCollection = EditorWindowUtils.GetLanguages();

            var localizationKeyCollection = _table.GetRelatedKeys();
            _relatedKey = localizationKeyCollection.GetKey(key);
            
           _table.TryGetTranslateInfo(key, out _info);
            
            var visualTree =
                AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(
                    $"{EditorUtils.PACKAGE_EDITOR_PATH}/Editor Window/Tables Window/EditTableKeyItem.uxml");
            visualTree.CloneTree(this);
            
            _keyLabel = this.Q<Label>("key-label");
            _keyLabel.text = key;

            CreateTranslateFields();
        }

        void CreateTranslateFields()
        {
            var template = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(
                $"{EditorUtils.PACKAGE_EDITOR_PATH}/Editor Window/Tables Window/EditTranslateKeyItem.uxml");

            var box = this.Q<GroupBox>("key-languages-box");
            
            var translateType = _relatedKey.Type;
            var count = _info.text.Count;
            
            for (int i = 0; i < count; i++)
            {
                var item = template.Instantiate();
                BindTranslateField(item, i, translateType, i);
                
                box.Add(item);
            }
        }

        void BindTranslateField(VisualElement element, int languageId, LocalizationKey.KeyType keyType, int index)
        {
            var languageLabel = element.Q<Label>("language-label");
            var languageImage = element.Q<UxmlImage>("language-image");

            var languageKey = _languageKeyCollection.GetKeyByID(languageId);
            languageLabel.text = languageKey.name;
            languageImage.sprite = languageKey.icon;
            
            var itemField = element.Q<VisualElement>("item-field");
            
            var serializedObject = new SerializedObject(_table);
            var infoProperty = serializedObject
                .FindProperty("_translation")
                .GetArrayElementAtIndex(_mainIndex)
                .FindPropertyRelative("translateInfo");

            VisualElement field;
            if (keyType == LocalizationKey.KeyType.Text)
            {
                var textProperty = infoProperty.FindPropertyRelative("text").GetArrayElementAtIndex(index);
                var text = textProperty.FindPropertyRelative("text");

                var textField = new TextField();
                textField.value = text.stringValue;

                textField.BindProperty(text);
                field = textField;
            }
            else if (keyType == LocalizationKey.KeyType.Sprite)
            {
                var spriteProperty = infoProperty.FindPropertyRelative("sprite").GetArrayElementAtIndex(index);
                var sprite = spriteProperty.FindPropertyRelative("sprite");

                var objectField = new ObjectField();
                objectField.objectType = typeof(Sprite);
                objectField.value = sprite.objectReferenceValue;

                objectField.BindProperty(sprite);
                field = objectField;
            }
            else if (keyType == LocalizationKey.KeyType.Texture)
            {
                var textureProperty = infoProperty.FindPropertyRelative("texture").GetArrayElementAtIndex(index);
                var texture = textureProperty.FindPropertyRelative("texture");

                var objectField = new ObjectField();
                objectField.objectType = typeof(Texture);
                objectField.value = texture.objectReferenceValue;

                objectField.BindProperty(texture);
                field = objectField;
            }
            else
            {
                throw new System.NotImplementedException();
            }
            
            field.style.unityTextAlign = TextAnchor.MiddleLeft;
            field.StretchToParentSize();
            
            itemField.Add(field);
            itemField.Bind(serializedObject);
        }
    }
}