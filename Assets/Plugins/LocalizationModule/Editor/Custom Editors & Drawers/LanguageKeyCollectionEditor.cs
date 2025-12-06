using LocalizationModule.Runtime;
using UnityEditor;
using UnityEngine;

namespace LocalizationModule.Editor
{
    [CustomEditor(typeof(LanguageKeyCollection))]
    internal class LanguageKeyCollectionEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            EditorGUI.BeginChangeCheck();
            serializedObject.Update();
            
            DrawDefaultInspector();
            DrawOpenEditorWindow();
            
            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
            }
        }
        
        void DrawOpenEditorWindow()
        {
            if (GUILayout.Button("Open editor window"))
            {
                LanguageKeysWindow.OpenWindow();
            }
        }
    }
}