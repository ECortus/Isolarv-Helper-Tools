using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace IsolarvHelperTools.Editor
{
    public abstract class CustomEditorModule : UnityEditor.Editor
    {
        protected virtual void OnEnable()
        {
            
        }
        
        protected virtual string[] GetPropertiesToExclude() => Array.Empty<string>();
        protected abstract void OnEditorDraw();
        
        public override void OnInspectorGUI()
        {
            GUI.enabled = false;
            DrawProperty("m_Script");
            GUI.enabled = true;
            
            serializedObject.Update();
            
            OnEditorDraw();
            Space();

            var propertiesToExclude = GetPropertiesToExclude();
            
            var toExclude = new string[propertiesToExclude.Length + 1];
            toExclude[0] = "m_Script";
            Array.Copy(propertiesToExclude, 0, toExclude, 1, propertiesToExclude.Length);
            
            DrawPropertiesExcluding(toExclude.ToList());
            
            serializedObject.ApplyModifiedProperties();
            serializedObject.Update();
        }
        
        void DrawPropertiesExcluding(List<string> propertiesToExclude)
        {
            var props = serializedObject.GetIterator();
            props.NextVisible(true);
            
            while (props.NextVisible(false))
            {
                if (propertiesToExclude.Contains(props.name))
                    continue;
                
                EditorGUILayout.PropertyField(props, true);
            }
        }
        
        protected SerializedProperty FindProperty(string propertyName)
        {
            return serializedObject.FindProperty(propertyName);
        }
        
        protected SerializedProperty FindPropertyRelative(SerializedProperty property, string propertyName)
        {
            return property.FindPropertyRelative(propertyName);
        }
        
        protected void DrawProperty(SerializedProperty property)
        {
            Rect rect = EditorGUILayout.GetControlRect();
            EditorGUI.PropertyField(rect, property, true);
        }
        
        protected void DrawProperty(string propertyName)
        {
            var property = serializedObject.FindProperty(propertyName);
            if (property != null)
            {
                EditorGUILayout.PropertyField(property);
            }
            else
            {
                Debug.LogError("Cannot find property with name: " + propertyName);
            }
        }
        
        protected bool GetBoolValue(SerializedProperty property)
        {
            return property.boolValue;
        }
        
        protected void Space(int height = 10)
        {
            GUILayout.Space(height);
        }
    }
}