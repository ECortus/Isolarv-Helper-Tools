using System;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace IsolarvHelperTools
{
    public enum EComparisonType
    {
        Equals = 1,
        NotEqual = 2,
        GreaterThan = 3,
        SmallerThan = 4,
        SmallerOrEqual = 5,
        GreaterOrEqual = 6
    }
    
    public enum EDisablingType
    {
        ReadOnly = 2,
        DontDraw = 3
    }
    
    /// <summary>
    /// Draws the field/property ONLY if the copared property compared by the comparison type with the value of comparedValue returns true.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true)]
    public class DrawIfAttribute : PropertyAttribute
    {
        public string comparedPropertyName { get; private set; }
        public object comparedValue { get; private set; }
        
        public EComparisonType comparisonType { get; private set; }
        public EDisablingType disablingType { get; private set; }
 
        /// <summary>
        /// Only draws the field only if a condition is met.
        /// </summary>
        /// <param name="comparedPropertyName">The name of the property that is being compared (case sensitive).</param>
        /// <param name="comparedValue">The value the property is being compared to.</param>
        /// <param name="comparisonType">The type of comperison the values will be compared by.</param>
        /// <param name="disablingType">The type of disabling that should happen if the condition is NOT met. Defaulted to DisablingType.DontDraw.</param>
        public DrawIfAttribute(string comparedPropertyName, object comparedValue, EDisablingType disablingType = EDisablingType.DontDraw)
        {
            this.comparedPropertyName = comparedPropertyName;
            this.comparedValue = comparedValue;
            this.comparisonType = comparisonType;
            this.disablingType = disablingType;
        }
    }

#if UNITY_EDITOR
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
                Debug.LogError("Cannot find property with name: " + path);
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
                    Debug.LogError("Error of attribute helper: " + comparedField.type + " is not supported.");
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

#endif    
}