using UnityEngine;

namespace GameDevUtils.Runtime
{
    public static class ObjectHelper
    {
        public static void DontDestroyOnLoad(GameObject obj)
        {
            DebugHelper.Log("DontDestroyOnLoad object: " + obj);

            obj.transform.parent = null;
            GameObject.DontDestroyOnLoad(obj);
        }
        
        public static void Destroy(Object obj)
        {
            DebugHelper.Log("Destroying object: " + obj);
            GameObject.Destroy(obj);
        }
        
        public static void Destroy(Object obj, float time)
        {
            DebugHelper.Log("Destroying object: " + obj + " in " + time + " seconds");
            GameObject.Destroy(obj, time);
        }
        
        public static void DestroyImmediate(Object obj)
        {
            DebugHelper.Log("Immediate destroying object: " + obj);
            GameObject.DestroyImmediate(obj);
        }
    }
}