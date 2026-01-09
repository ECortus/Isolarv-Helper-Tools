using System;
using LocalizationModule.Runtime;
using UnityEditor;
using UnityEngine;

namespace LocalizationModule.Editor
{
    [CustomEditor(typeof(TranslateTable), true), CanEditMultipleObjects]
    internal class TranslateTableEditor : UnityEditor.Editor
    {
        TranslateTable _translateTable;

        private void OnEnable()
        {
            _translateTable = target as TranslateTable;
        }

        public override void OnInspectorGUI()
        {
            EditorGUI.BeginChangeCheck();
            serializedObject.Update();
            
            DrawScript();
            
            DrawKeyVariable();
            DrawDebugButtons();
            
            DrawOpenEditorWindow();

            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
            }
        }

        void DrawScript()
        {
            GUI.enabled = false;
            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_Script"));
            GUI.enabled = true;
        }

        void DrawKeyVariable()
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("relatedKeys"));
        }

        void DrawDebugButtons()
        {
            if (GUILayout.Button("Clear table"))
            {
                _translateTable.translation.Clear();
            }
        }
        
        void DrawOpenEditorWindow()
        {
            if (GUILayout.Button("Open editor window"))
            {
                TranslateTablesWindow.OpenWindow();
            }
        }
    }
}