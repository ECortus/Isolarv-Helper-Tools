using UnityEngine;

namespace Plugins.GameDevUtils.Runtime.Extensions
{
    public static class GameObjectExtensions
    {
        public static T GetOrAddComponent<T>(this MonoBehaviour go) where T : Component
        {
            return GetOrAddComponent<T>(go.gameObject);
        }
        
        public static T GetOrAddComponent<T>(this GameObject go) where T : Component
        {
            if (go.TryGetComponent(out T component))
            {
                return component;
            }

            var newComponent = go.AddComponent<T>();
            return newComponent;
        }
    }
}