using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LocalizationModule.Runtime;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace LocalizationModule.Editor
{
    internal class LocalizationKeysWindow : EditorWindow
    {
        private List<LocalizationKeyCollection> _localizationKeyCollections;

        private GroupBox _contentBox;
        
        private ListView _localizationListView;
        private VisualTreeAsset _listViewItemTemplate;

        private List<LocalizationKeyCollection> _keysSet;

        public event Action OnUpdate;
        
        [MenuItem("Tools/Isolarv/Localization Tool/Keys", false, 60)]
        public static void ShowWindow()
        {
            EditorWindowUtils.ShowWindow<LocalizationKeysWindow>("Keys");
        }

        public static void OpenWindow(params Type[] desiredDockNextTo)
        {
            EditorWindowUtils.OpenWindow<LocalizationKeysWindow>("Keys", desiredDockNextTo);;
        }

        public void CreateGUI()
        {
            TryLoadCollection();
            
            VisualElement root = rootVisualElement;
            
            var visualTree =
                AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(
                    $"{EditorUtils.PACKAGE_EDITOR_PATH}/Editor Window/Keys Window/LocalizationKeysWindow.uxml");

            VisualElement labelFromUxml = visualTree.Instantiate();
            root.Add(labelFromUxml);
            
            UpdateKeysSet();

            AddNewLocalizationCollectionGUI();
            CreateListViewGUI();
            CreateContentBox();
        }
        
        int _lastCount = -1;

        void Update()
        {
            TryLoadCollection();
            if (_localizationKeyCollections.Count != _lastCount)
            {
                UpdateOnCollectionChange();
            }
            
            OnUpdate?.Invoke();
        }

        void TryLoadCollection()
        {
            _localizationKeyCollections = EditorUtils.LoadAllAssetsInFolder<LocalizationKeyCollection>(EditorUtils.KEYS_PATH).ToList();
        }

        void UpdateOnCollectionChange()
        {
            ClearSelection();
            UpdateKeysSet();
        }
        
        internal void UpdateKeysSet()
        {
            _keysSet = _localizationKeyCollections;
            _lastCount = _keysSet.Count;
            
            if (_localizationListView != null)
            {
                _localizationListView.itemsSource = _keysSet;
                _localizationListView.Rebuild();
            }
        }

        TextField _newCollectionNameField;
        Label _previewCollectionName;
        
        string _newCollectionName = "";
        
        void AddNewLocalizationCollectionGUI()
        {
            _newCollectionNameField = rootVisualElement.Q<TextField>("new-collection-field");
            _previewCollectionName = rootVisualElement.Q<Label>("preview-collection-name");
            
            _newCollectionNameField.RegisterValueChangedCallback(evt =>
            {
                _newCollectionName = evt.newValue + "_KeyCollection";
                _previewCollectionName.text = $"Preview name:\n" + _newCollectionName;
            });
            
            var addLocalizationButton = rootVisualElement.Q<Button>("add-localization-button");
            addLocalizationButton.clicked += CreateNewLocalizationCollection;
        }
        
        void CreateNewLocalizationCollection()
        {
            if (_newCollectionName == "" || _localizationKeyCollections.Any(x => x.name == _newCollectionName))
            {
                var color = Color.red;
                color.a = 0.5f;

                _newCollectionNameField.style.backgroundColor = color;
                return;
            }
            else
            {
                _newCollectionNameField.style.backgroundColor = Color.clear;
            }
            
            var newCollection = ScriptableObject.CreateInstance<LocalizationKeyCollection>();
            newCollection.name = _newCollectionName;

            var assetPath = $"{EditorUtils.KEYS_PATH}/{_newCollectionName}.asset";
            EditorUtils.CreateAsset(newCollection, assetPath);

            _newCollectionNameField.value = "";
            UpdateKeysSet();
        }

        void CreateListViewGUI()
        {
            _localizationListView = rootVisualElement.Q<ListView>("localization-list-view");
            
            _listViewItemTemplate = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(
                $"{EditorUtils.PACKAGE_EDITOR_PATH}/Editor Window/Keys Window/LocalizationListViewItem.uxml");
            
            _localizationListView.itemsSource = _keysSet;
            _localizationListView.selectionType = SelectionType.Single;
            _localizationListView.selectionChanged += ListViewSelectionChanged;
            _localizationListView.showBoundCollectionSize = true;
            _localizationListView.makeItem = () => _listViewItemTemplate.Instantiate();
            _localizationListView.bindItem = BindListViewItem;
#if UNITY_2021_3_OR_NEWER
            _localizationListView.virtualizationMethod = CollectionVirtualizationMethod.DynamicHeight;
#endif
        }   
        
        void CreateContentBox()
        {
            _contentBox = rootVisualElement.Q<GroupBox>("content-box");
        }
        
        int _lastSelectedIndex = -1;
        
        private void ListViewSelectionChanged(IEnumerable<object> objects)
        {
            if (objects == null || !objects.Any()) return;
            if (_lastSelectedIndex == _localizationListView.selectedIndex) return;
            
            _lastSelectedIndex = _localizationListView.selectedIndex;
            OnLanguageSelected(_lastSelectedIndex);
        }

        internal void OnLanguageSelected(int index)
        {
            _localizationListView.selectedIndex = index;
            _contentBox.Clear();

            var collection = _localizationKeyCollections[index];
            _contentBox.Add(new EditSelectedLocalizationKeys(collection, this));
        }

        internal void ClearSelection()
        {
            _lastSelectedIndex = -1;
            
            _contentBox.Clear();
            _localizationListView.ClearSelection();
        }
        
        private void BindListViewItem(VisualElement listItem, int i)
        {
            var key = _keysSet[i];
            if (key == null) return;
            
            var mainAbilityBox = listItem.GetFirstOfType<VisualElement>();
            listItem.userData = i.ToString();
            
            var nameLabel = mainAbilityBox.Q<Label>("Localization-Label");
            nameLabel.text = key.name;

            var icon = AssetDatabase.LoadAssetAtPath<Sprite>($"{EditorUtils.PACKAGE_BASE_PATH}/Sprites/scriptable_icon.png");
            var abilityImage =  mainAbilityBox.Q<UxmlImage>("Localization-Image");
            abilityImage.image = icon.texture;
            
            var removeButton = mainAbilityBox.Q<Button>("remove-button");
            removeButton.clicked += () =>
            {
                AssetDatabase.DeleteAsset($"{EditorUtils.KEYS_PATH}/{key.name}.asset");
                UpdateKeysSet();
            };
        }
    }
}
