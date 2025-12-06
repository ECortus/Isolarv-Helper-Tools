using LocalizationModule.Runtime;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace LocalizationModule.Editor
{
    internal class EditSelectedLanguage : VisualElement
    {
        LanguageKeyCollection _keyCollection;
        int _index;
        LanguageKeysWindow _mainWindow;
        
        public EditSelectedLanguage(LanguageKeyCollection keyCollection, int index, LanguageKeysWindow mainWindow)
        {
            _mainWindow = mainWindow;
            _keyCollection = keyCollection;
            _index = index;
            
            var visualTree =
                AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(
                    $"{EditorUtils.PACKAGE_EDITOR_PATH}/Editor Window/Language Window/LanguageEditWindow.uxml");
            visualTree.CloneTree(this);
            
            var contentBox = this.Q<GroupBox>("fields-box");
            
            var removeButton = this.Q<Button>("remove-language-button");
            removeButton.clicked += OnRemoveButtonClicked;
            
            var serialized = new SerializedObject(_keyCollection);
            var keysCollection = serialized.FindProperty("keys");
            var keyProperty = keysCollection.GetArrayElementAtIndex(_index);
            
            var idProperty = keyProperty.FindPropertyRelative("id");
            var nameProperty = keyProperty.FindPropertyRelative("name");
            var spriteProperty = keyProperty.FindPropertyRelative("icon");
            
            contentBox.Add(new PropertyField(idProperty));
            contentBox.Add(new PropertyField(nameProperty));
            contentBox.Add(new PropertyField(spriteProperty));
            
            contentBox.Bind(serialized);
        }
        
        void OnRemoveButtonClicked()
        {
            _keyCollection.RemoveKey(_index);
            
            _mainWindow.UpdateKeysSet();
            _mainWindow.ClearSelection();
        }
    }
}