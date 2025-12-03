using UnityEngine;

namespace GameDevUtils.Runtime.Colliders
{
    public static class ColliderHelper
    {
        public static bool IsSameMask(this Collider collider, string name)
        {
            return IsSameMask(collider.gameObject, name);
        }
        
        public static bool IsSameMaskBitwise(this Collider collider, string name)
        {
            return IsSameMaskBitwise(collider.gameObject, name);
        }

        public static bool IsSameMask(this GameObject collider, string name)
        {
            return collider.gameObject.layer == LayerMask.NameToLayer(name);
        }
        
        public static bool IsSameMaskBitwise(this GameObject collider, string name)
        {
            return collider.gameObject.layer == 1 << LayerMask.NameToLayer(name);
        }
    }
}