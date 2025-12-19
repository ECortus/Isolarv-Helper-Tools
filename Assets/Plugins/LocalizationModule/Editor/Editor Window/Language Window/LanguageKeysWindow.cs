using System;
using System.Collections.Generic;
using System.Linq;
using LocalizationModule.Runtime;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace LocalizationModule.Editor
{
    internal class LanguageKeysWindow : EditorWindow
    {
        private LanguageKeyCollection _languageKeyCollection;

        private GroupBox _contentBox;
        
        private ListView _languageListView;
        private VisualTreeAsset _listViewItemTemplate;

        private List<LanguageKey> _keysSet;

        public static void OpenWindow(params Type[] desiredDockNextTo)
        {
            EditorWindowUtils.OpenWindow<LanguageKeysWindow>("Languages", desiredDockNextTo);
        }

        public void CreateGUI()
        {
            _languageKeyCollection = AssetDatabase.LoadAssetAtPath<LanguageKeyCollection>(
                $"{LocalizationEditorUtils.PACKAGE_BASE_PATH}/Data/Language Key Collection.asset");
            
            VisualElement root = rootVisualElement;

            var visualTree =
                AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(
                    $"{LocalizationEditorUtils.PACKAGE_EDITOR_PATH}/Editor Window/Language Window/LanguageKeysWindow.uxml");
            
            VisualElement labelFromUxml = visualTree.Instantiate();
            root.Add(labelFromUxml);
            
            UpdateKeysSet();

            AddNewLanguageGUI();
            CreateListViewGUI();
            CreateContentBox();
        }

        int _lastCount = -1;

        void Update()
        {
            if (_languageKeyCollection && _languageKeyCollection.keysCount != _lastCount)
            {
                UpdateOnCollectionChange();
            }
        }

        void UpdateOnCollectionChange()
        {
            ClearSelection();
            UpdateKeysSet();
        }

        internal void UpdateKeysSet()
        {
            _keysSet = _languageKeyCollection.GetKeys();
            _lastCount = _keysSet.Count;
            
            if (_languageListView != null)
            {
                _languageListView.itemsSource = _keysSet;
                _languageListView.Rebuild();
            }
        }

        void AddNewLanguageGUI()
        {
            var addLanguageButton = rootVisualElement.Q<Button>("add-language-button");
            addLanguageButton.clicked += SwitchToCreateNewLanguageView;
        }
        
        void SwitchToCreateNewLanguageView()
        {
            ClearSelection();
            _contentBox.Add(new AddNewLanguage(_languageKeyCollection, this));
        }

        void CreateListViewGUI()
        {
            _languageListView = rootVisualElement.Q<ListView>("language-list-view");
            
            _listViewItemTemplate = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(
                $"{LocalizationEditorUtils.PACKAGE_EDITOR_PATH}/Editor Window/Language Window/LanguageListViewItem.uxml");
            
            _languageListView.itemsSource = _keysSet;
            _languageListView.selectionType = SelectionType.Single;
            _languageListView.selectionChanged += ListViewSelectionChanged;
            _languageListView.showBoundCollectionSize = true;
            _languageListView.makeItem = () => _listViewItemTemplate.Instantiate();
            _languageListView.bindItem = BindListViewItem;
#if UNITY_2021_3_OR_NEWER
            _languageListView.virtualizationMethod = CollectionVirtualizationMethod.DynamicHeight;
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
            if (_lastSelectedIndex == _languageListView.selectedIndex) return;
            
            _lastSelectedIndex = _languageListView.selectedIndex;
            OnLanguageSelected(_lastSelectedIndex);
        }

        internal void OnLanguageSelected(int index)
        {
            _languageListView.selectedIndex = index;
            
            _contentBox.Clear();
            _contentBox.Add(new EditSelectedLanguage(_languageKeyCollection, index, this));
        }

        internal void ClearSelection()
        {
            _lastSelectedIndex = -1;
            
            _contentBox.Clear();
            _languageListView.ClearSelection();
        }
        
        private void BindListViewItem(VisualElement listItem, int i)
        {
            var key = _keysSet[i];
            if (key == null) return;
            
            var mainAbilityBox = listItem.GetFirstOfType<VisualElement>();
            listItem.userData = i.ToString();
            
            var nameLabel = mainAbilityBox.Q<Label>("Language-Label");
            nameLabel.text = key.name;

            var abilityImage =  mainAbilityBox.Q<UxmlImage>("Language-Image");
            if (key.icon)
            {
                abilityImage.image = key.icon.texture;
            }
        }
    }
}
