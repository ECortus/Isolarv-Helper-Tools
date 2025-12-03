using UnityEngine;

namespace GameDevUtils.Runtime.Colliders
{
    public static class ColliderHelper
    {
        public static bool IsSameMask(this Collider collider, string name)
        {
            return collider.gameObject.layer == 1 << LayerMask.NameToLayer(name);
        }
    }
}