using UnityEngine;

namespace GameDevUtils.Runtime.Extensions
{
    public static class DetectingExtensions
    {
        public static bool IsSameMask(this Collider collider, string name)
        {
            return IsSameMask(collider.gameObject, name);
        }
        
        public static bool IsSameMaskBitwise(this Collider collider, string name)
        {
            return IsSameMaskBitwise(collider.gameObject, name);
        }
        
        public static bool IsSameMask(this Collision collision, string name)
        {
            return IsSameMask(collision.gameObject, name);
        }
        
        public static bool IsSameMaskBitwise(this Collision collision, string name)
        {
            return IsSameMaskBitwise(collision.gameObject, name);
        }

        static bool IsSameMask(this GameObject obj, string name)
        {
            return obj.layer == LayerMask.NameToLayer(name);
        }
        
        static bool IsSameMaskBitwise(this GameObject obj, string name)
        {
            return obj.layer == 1 << LayerMask.NameToLayer(name);
        }
    }
}