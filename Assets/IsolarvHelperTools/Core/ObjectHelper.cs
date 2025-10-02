using UnityEngine;

namespace IsolarvHelperTools
{
    public static class ObjectHelper
    {
        public static void Destroy(Object obj)
        {
            // Debug.Log("Destroying object: " + obj);
            GameObject.Destroy(obj);
        }
        
        public static void Destroy(Object obj, float time)
        {
            // Debug.Log("Destroying object: " + obj + " in " + time + " seconds");
            GameObject.Destroy(obj, time);
        }
        
        public static void DestroyImmediate(Object obj)
        {
            // Debug.Log("Immediate destroying object: " + obj);
            GameObject.DestroyImmediate(obj);
        }
        
        
    }
}