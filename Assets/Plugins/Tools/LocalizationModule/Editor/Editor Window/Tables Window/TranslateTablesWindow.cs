using System;
using LocalizationModule.Runtime;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace LocalizationModule.Editor
{
    internal class TranslateTablesWindow : EditorWindow
    {
        static LocalizationKeyCollection _selectedCollection;
        static TranslateTable _selectedTable;

        ScrollView _tableView;
        
        public static void OpenWindow(params Type[] desiredDockNextTo)
        {
            EditorWindowUtils.OpenWindow<TranslateTablesWindow>("Tables", desiredDockNextTo);
        }

        public void CreateGUI()
        {
            VisualElement root = rootVisualElement;

            var visualTree =
                AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(
                    $"{LocalizationEditorUtils.PACKAGE_EDITOR_PATH}/Editor Window/Tables Window/TranslateTablesWindow.uxml");
            
            VisualElement labelFromUxml = visualTree.Instantiate();
            root.Add(labelFromUxml);
            
            _tableView = rootVisualElement.Q<ScrollView>("table-view");

            CreateCollectionSelectField();
            CreateTableView();
        }

        void CreateCollectionSelectField()
        {
            var objectField = new ObjectField();
            objectField.objectType = typeof(LocalizationKeyCollection);
            
            if (_selectedCollection != null)
            {
                objectField.value = _selectedCollection;
            }
            
            objectField.RegisterValueChangedCallback((loc) =>
            {
                _selectedCollection = loc.newValue as LocalizationKeyCollection;
                if (_selectedCollection)
                {
                    _selectedTable = _selectedCollection.Table;
                }
                
                UpdateTableView();
            });
            
            var keysField = rootVisualElement.Q<GroupBox>("keys-field");
            keysField.Add(objectField);
        }
        
        void UpdateTableView()
        {
            _tableView.Clear();
            CreateTableView();
        }

        void CreateTableView()
        {
            if (_selectedTable)
            {
                EditorWindowUtils.ValidateTable(_selectedTable);
            }
            
            var emptyTemplate = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(
                $"{LocalizationEditorUtils.PACKAGE_EDITOR_PATH}/Editor Window/Tables Window/TableEmptyItem.uxml");

            string emptyLabel = "";
            bool isEmpty = false;
            
            if (_selectedCollection == null)
            {
                emptyLabel = "No collection selected.";
                isEmpty = true;
            }
            else if (_selectedCollection.keysCount == 0)
            {
                emptyLabel = "No keys in collection.";
                isEmpty = true;
            }
            
            if (isEmpty)
            {
                var emptyItem = emptyTemplate.Instantiate();
                emptyItem.Q<Label>("empty-label").text = emptyLabel;
                
                _tableView.Add(emptyItem);
                return;
            }
            
            for (int i = 0; i < _selectedTable.translation.Count; i++)
            {
                var key = _selectedTable.translation[i].GetKey();
                AddEditTableItem(key, i);
            }
        }
        
        void AddEditTableItem(string key, int index)
        {
            var edit = new EditTableKeyWindow(_selectedTable, key, index, this);
            _tableView.Add(edit);
        }
    }
}
