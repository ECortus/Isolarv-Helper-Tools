using LocalizationModule.Runtime;
using UnityEditor;
using UnityEngine;

namespace LocalizationModule.Editor
{
    [CustomEditor(typeof(LocalizationKeyCollection))]
    internal class LocalizationKeyCollectionEditor : UnityEditor.Editor
    {
        LocalizationKeyCollection collection;
        
        private void OnEnable()
        {
            collection = (LocalizationKeyCollection)target;
        }
        
        public override void OnInspectorGUI()
        {
            EditorGUI.BeginChangeCheck();
            serializedObject.Update();

            DrawMainGUI();
            
            EditorGUILayout.Space(5);
            DrawCollectionButtons();

            DrawOpenEditorWindow();
            
            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
            }
        }

        void DrawMainGUI()
        {
            GUI.enabled = false;
            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_Script"));
            GUI.enabled = true;
            
            EditorGUILayout.PropertyField(serializedObject.FindProperty("keys"), true);
            
            GUI.enabled = false;
            EditorGUILayout.PropertyField(serializedObject.FindProperty("relatedTable"));
            GUI.enabled = true;
        }

        void DrawCollectionButtons()
        {
            if (!collection.ScanForDuplicate())
            {
                GUI.enabled = false;
            }
            
            if (GUILayout.Button("Remove duplicates"))
            {
                collection.RemoveDuplicates();
            }
            
            GUI.enabled = true;
            
            if (GUILayout.Button("Validate Table"))
            {
                EditorUtils.ValidateTableOfKeys(collection);
            }
        }
        
        void DrawOpenEditorWindow()
        {
            if (GUILayout.Button("Open editor window"))
            {
                LocalizationKeysWindow.OpenWindow();
            }
        }
    }
}