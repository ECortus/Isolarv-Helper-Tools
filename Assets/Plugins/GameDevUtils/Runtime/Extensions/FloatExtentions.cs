using UnityEngine;

namespace Plugins.GameDevUtils.Runtime.Extensions
{
    public static class FloatExtentions
    {
        public static bool Equal(this float value, float other)
        {
            return Mathf.Approximately(value, other);
        }
    }
}