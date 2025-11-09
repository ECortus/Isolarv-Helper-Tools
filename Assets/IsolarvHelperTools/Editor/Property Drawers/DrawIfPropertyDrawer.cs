using IsolarvHelperTools.Runtime;
using UnityEditor;
using UnityEngine;

namespace IsolarvHelperTools.Editor
{
    [CustomPropertyDrawer(typeof(DrawIfAttribute))]
    public class DrawIfPropertyDrawer : PropertyDrawer
    {
        #region Fields

        // Reference to the attribute on the property.
        DrawIfAttribute drawIf;

        // Field that is being compared.
        SerializedProperty comparedField;

        #endregion

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (!ShowMe(property) && drawIf.disablingType == EDisablingType.DontDraw)
                return 0f;

            // The height of the property should be defaulted to the default height.
            return base.GetPropertyHeight(property, label);
        }

        /// <summary>
        /// Errors default to showing the property.
        /// </summary>
        private bool ShowMe(SerializedProperty property)
        {
            drawIf = attribute as DrawIfAttribute;
            if (drawIf == null)
                return false;
            
            // Replace propertyname to the value from the parameter
            string path = property.propertyPath.Contains(".")
                ? System.IO.Path.ChangeExtension(property.propertyPath, drawIf.comparedPropertyName)
                : drawIf.comparedPropertyName;

            comparedField = property.serializedObject.FindProperty(path);

            if (comparedField == null)
            {
                DebugHelper.LogError("Cannot find property with name: " + path);
                return true;
            }
            
            var value = CompareFieldAndObject();
            return value;
        }
        
        bool CompareFieldAndObject()
        {
            object comparedValue = drawIf.comparedValue;
            switch (comparedField.type)
            {
                case "bool":
                    return comparedField.boolValue.Equals(comparedValue);
                case "Enum":
                    return comparedField.enumValueIndex.Equals((int)comparedValue);
                default:
                    DebugHelper.LogError("Error of attribute helper: " + comparedField.type + " is not supported.");
                    return true;
            }
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // If the condition is met, simply draw the field.
            if (ShowMe(property))
            {
                EditorGUI.PropertyField(position, property);
            } //...check if the disabling type is read only. If it is, draw it disabled
            else if (drawIf.disablingType == EDisablingType.ReadOnly)
            {
                GUI.enabled = false;
                EditorGUI.PropertyField(position, property);
                GUI.enabled = true;
            }
        }
    }
}