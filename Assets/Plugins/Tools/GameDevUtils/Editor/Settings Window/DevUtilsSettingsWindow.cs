using GameDevUtils.Editor;
using GameDevUtils.Runtime.Settings;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace GameSaveKit.Editor.CustomWindows
{
    internal class DevUtilsSettingsWindow : EditorWindow
    {
        [MenuItem("Tools/Isolarv/Game Dev Utils/Settings", false, 10)]
        public static void ShowWindow()
        {
            EditorWindowUtils.ShowWindow<DevUtilsSettingsWindow>("Dev Utils Settings");
        }
        
        public void CreateGUI()
        {
            var visualTree =
                AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(
                    $"{DevUtilsEditorUtils.PACKAGE_EDITOR_PATH}/Settings Window/DevUtilsSettingsWindow.uxml");
            
            VisualElement root = rootVisualElement;

            VisualElement labelFromUXML = visualTree.Instantiate();
            root.Add(labelFromUXML);

            InitParametersFields();
        }

        void InitParametersFields()
        {
            var enableLoadingScreen = rootVisualElement.Q<Toggle>("enable-loading-screen");
            enableLoadingScreen.label = "Enable Loading Screen";
            enableLoadingScreen.value = DevUtilsParametersSettings.EnableLoadingScreen;
            enableLoadingScreen.RegisterValueChangedCallback(evt => DevUtilsParametersSettings.EnableLoadingScreen = evt.newValue);
            
            var savingTypeField = rootVisualElement.Q<EnumField>("load-scene-mode");
            savingTypeField.label = "Load Scene Mode";
            savingTypeField.dataSourceType = typeof(LoadSceneMode);
            savingTypeField.value = DevUtilsParametersSettings.LoadSceneMode;
            savingTypeField.RegisterValueChangedCallback(evt => 
                DevUtilsParametersSettings.LoadSceneMode = (LoadSceneMode)evt.newValue);
        }
    }
}
