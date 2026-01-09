using LocalizationModule.Runtime;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace LocalizationModule.Editor
{
    internal class EditSelectedLocalizationKeys : VisualElement
    {
        LocalizationKeyCollection _localizationKeyCollection;
        LocalizationKeysWindow _parentWindow;
        
        SerializedObject _serializedObject;
        
        Button _removeDuplicatesButton;
        ScrollView _scrollView;
        
        public EditSelectedLocalizationKeys(LocalizationKeyCollection localizationKeyCollection, LocalizationKeysWindow parentWindow)
        {
            _localizationKeyCollection = localizationKeyCollection;
            _parentWindow = parentWindow;
            
            var visualTree =
                AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(
                    $"{LocalizationEditorUtils.PACKAGE_EDITOR_PATH}/Editor Window/Keys Window/LocalizationEditWindow.uxml");
            visualTree.CloneTree(this);
            
            _scrollView = this.Q<ScrollView>("scroll-view");
            CreateScrollView();
            
            var addNewKeyButton = this.Q<Button>("add-new-key");
            addNewKeyButton.clicked += () =>
            {
                _localizationKeyCollection.AddKey(new LocalizationKey("NEW-KEY", LocalizationKey.KeyType.Text));
            };
            
            var validateTableButton = this.Q<Button>("validate-table");
            validateTableButton.clicked += () =>
            {
                LocalizationEditorUtils.ValidateTableOfKeys(_localizationKeyCollection);
            };

            _removeDuplicatesButton = this.Q<Button>("remove-duplicates");
            _removeDuplicatesButton.clicked += () =>
            {
                _localizationKeyCollection.RemoveDuplicates();
            };
            
            _parentWindow.OnUpdate += Update;
        }

        int _lastCount = -1;

        void Update()
        {
            if (!_localizationKeyCollection)
            {
                return;
            }
            
            if (_lastCount != _localizationKeyCollection.keysCount)
            {
                UpdateScrollView();
            }

            if (_localizationKeyCollection.ScanForDuplicate())
            {
                _removeDuplicatesButton.SetEnabled(true);
            }
            else
            {
                _removeDuplicatesButton.SetEnabled(false);
            }
        }
        
        void UpdateScrollView()
        {
            _scrollView.Clear();
            CreateScrollView();
        }

        void CreateScrollView()
        {
            if (!_localizationKeyCollection)
            {
                return;
            }
            
            _serializedObject = new SerializedObject(_localizationKeyCollection);
            
            var keysProperty = _serializedObject.FindProperty("keys");
            if (keysProperty.arraySize > 0)
            {
                _lastCount = _localizationKeyCollection.keysCount;
                for (int i = 0; i < keysProperty.arraySize; i++)
                {
                    AddScrollItem(i);
                }
            }
            else
            {
                _lastCount = -1;
                CreateEmptyLabel();
            }
            
            _scrollView.Bind(_serializedObject);
        }

        void CreateEmptyLabel()
        {
            var template = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(
                $"{LocalizationEditorUtils.PACKAGE_EDITOR_PATH}/Editor Window/Keys Window/LocalizationEditEmptyItem.uxml");
            
            var item = template.Instantiate();
            _scrollView.Add(item);
        }

        void AddScrollItem(int index)
        {
            var template = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(
                $"{LocalizationEditorUtils.PACKAGE_EDITOR_PATH}/Editor Window/Keys Window/LocalizationKeyEditItem.uxml");
            
            var keysProperty = _serializedObject.FindProperty("keys");
            
            var item = template.Instantiate();
                
            var textField = item.Q<TextField>("default-key-field");
            var enumField = item.Q<EnumField>("type-field");

            var key = keysProperty.GetArrayElementAtIndex(index);

            var defaultKeyProperty = key.FindPropertyRelative("defaultKey");
            textField.BindProperty(defaultKeyProperty);
                
            var typeProperty = key.FindPropertyRelative("type");
            enumField.BindProperty(typeProperty);
                
            var removeButton = item.Q<Button>("remove-button");
            removeButton.clicked += () =>
            {
                _localizationKeyCollection.RemoveKey(index);
                _scrollView.Remove(item);
            };
                
            _scrollView.Add(item);
        }
    }
}