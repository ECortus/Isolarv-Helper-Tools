using UnityEngine;

namespace IsolarvHelperTools
{
    public static class ColliderHelper
    {
        public static bool IsPointInside(this Collider collider, Vector2 point, out RaycastHit hitInfo) 
        {
            var center = collider.bounds.center;
            var destination = (Vector3)point;
            
            var direction = center - destination;
            var ray = new Ray(point, direction);
            
            var hit = collider.Raycast(ray, out hitInfo, direction.magnitude);
            return !hit;
        }
    }
}