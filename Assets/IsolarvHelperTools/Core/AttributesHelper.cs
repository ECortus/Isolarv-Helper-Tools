using UnityEditor;
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
    
    public class AttributesHelper
    {
        public static bool CompareFieldAndObject(SerializedProperty comparedField, object comparedValue, 
            EComparisonType comparisonType = EComparisonType.Equals)
        {
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
    }
}