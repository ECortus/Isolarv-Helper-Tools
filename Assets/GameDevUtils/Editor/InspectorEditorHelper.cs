using UnityEditor;
using UnityEngine;

namespace GameDevUtils.Editor
{
    public static class InspectorEditorHelper
    {
        public static void DrawHeader(string label)
        {
            Space(5);
            EditorGUILayout.LabelField(label, EditorStyles.boldLabel);
            DrawSpaceLine();
        }
        
        public static void Space(int height = 10)
        {
            GUILayout.Space(height);
        }

        public static void DrawSpaceLine()
        {
            EditorGUI.DrawRect(EditorGUILayout.GetControlRect(false, 2), Color.gray);
        }
    }
}