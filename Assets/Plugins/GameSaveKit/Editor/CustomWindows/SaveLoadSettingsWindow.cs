using GameSaveKit.Runtime.Settings;
using UnityEditor;
using UnityEngine.UIElements;

namespace GameSaveKit.Editor.CustomWindows
{
    internal class SaveLoadSettingsWindow : EditorWindow
    {
        public void CreateGUI()
        {
            var visualTree =
                AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(
                    $"{GameSaveKitEditorUtils.PACKAGE_EDITOR_PATH}/CustomWindows/SaveLoadSettingsWindow.uxml");
            
            VisualElement root = rootVisualElement;

            VisualElement labelFromUXML = visualTree.Instantiate();
            root.Add(labelFromUXML);

            InitParametersFields();
        }

        void InitParametersFields()
        {
            var loadBehavioursOnAddingField = rootVisualElement.Q<Toggle>("load-behaviours-on-adding");
            loadBehavioursOnAddingField.label = "Load Behaviours On Adding";
            loadBehavioursOnAddingField.value = GameSaveParametersSettings.LoadBehavioursOnAdding;
            loadBehavioursOnAddingField.RegisterValueChangedCallback(evt => GameSaveParametersSettings.LoadBehavioursOnAdding = evt.newValue);
            
            var savingTypeField = rootVisualElement.Q<EnumField>("saving-type");
            savingTypeField.label = "Saving Type";
            savingTypeField.dataSourceType = typeof(GameSaveParametersSettings.ESavingType);
            savingTypeField.value = GameSaveParametersSettings.SavingType;
            savingTypeField.RegisterValueChangedCallback(evt => 
                GameSaveParametersSettings.SavingType = (GameSaveParametersSettings.ESavingType)evt.newValue);
        }
    }
}
