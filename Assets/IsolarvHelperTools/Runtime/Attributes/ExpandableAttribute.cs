using UnityEngine;

namespace IsolarvHelperTools.Runtime
{
    /// <summary>
    /// Use this property on a ScriptableObject type to allow the editors drawing the field to draw an expandable
    /// area that allows for changing the values on the object without having to change editor.
    /// </summary>
    public class ExpandableAttribute : PropertyAttribute
    {
        public bool ReadOnly { get; private set; }
        
        public ExpandableAttribute(bool readOnly = false)
        {
            ReadOnly = readOnly;
        }
    }
}