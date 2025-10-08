using UnityEngine;
using Zenject;

namespace IsolarvHelperTools.Runtime
{
    public class ObjectInstantiator : MonoBehaviour
    {
        static IInstantiator _instantiator;
        
        public void Init(DiContainer container)
        {
            _instantiator = container.Resolve<IInstantiator>();
        }
        
        public static T InstantiateComponentOnNewGameObject<T>(string gameObjectName = "", Transform parent = null) where T : Component
        {
            var instance = _instantiator.InstantiateComponentOnNewGameObject<T>(gameObjectName);
            instance.transform.SetParent(parent);
            
            instance.transform.localPosition = Vector3.zero;
            instance.transform.localRotation = Quaternion.identity;
            instance.transform.localScale = Vector3.one;
            
            return instance;
        }
        
        public static GameObject InstantiatePrefab(GameObject prefab)
        {
            return InstantiatePrefab(prefab, new Vector3(), new Quaternion(), null);
        }
        
        public static GameObject InstantiatePrefab(GameObject prefab, Transform parent)
        {
            return InstantiatePrefab(prefab, new Vector3(), new Quaternion(), parent);
        }
        
        public static GameObject InstantiatePrefab(GameObject prefab, Vector3 position, Quaternion rotation, Transform parent = null)
        {
            var instance = GameObject.Instantiate(prefab, position, rotation, parent);
            return instance;
        }
        
        public static GameObject InstantiatePrefab<T>(T prefab) where T : UnityEngine.Object
        {
            return InstantiatePrefab(prefab, new Vector3(), new Quaternion(), null);
        }
        
        public static GameObject InstantiatePrefab<T>(T prefab, Transform parent) where T : UnityEngine.Object
        {
            return InstantiatePrefab(prefab, new Vector3(), new Quaternion(), parent);
        }
        
        public static GameObject InstantiatePrefab<T>(T prefab, Vector3 position, Quaternion rotation, Transform parent = null) where T : UnityEngine.Object
        {
            var instance = GameObject.Instantiate(prefab, position, rotation, parent);
            if (instance is GameObject gameObj)
            {
                return gameObj;
            }
            
            if (instance is MonoBehaviour behaviour)
            {
                return behaviour.gameObject;
            }
            
            return null;
        }
        
        public static T InstantiatePrefabForComponent<T>(GameObject prefab) where T : UnityEngine.Object
        {
            return InstantiatePrefabForComponent<T>(prefab, new Vector3(), new Quaternion(), null);
        }
        
        public static T InstantiatePrefabForComponent<T>(GameObject prefab, Transform parent) where T : UnityEngine.Object
        {
            return InstantiatePrefabForComponent<T>(prefab, new Vector3(), new Quaternion(), parent);
        }
        
        public static T InstantiatePrefabForComponent<T>(GameObject prefab, Vector3 position, Quaternion rotation, Transform parent = null) where T : UnityEngine.Object
        {
            var instance = GameObject.Instantiate(prefab, position, rotation, parent);
            return instance.GetComponent<T>();
        }
        
        public static T InstantiatePrefabForComponent<T>(T prefab) where T : UnityEngine.Object
        {
            return InstantiatePrefabForComponent(prefab, new Vector3(), new Quaternion(), null);
        }
        
        public static T InstantiatePrefabForComponent<T>(T prefab, Transform parent) where T : UnityEngine.Object
        {
            return InstantiatePrefabForComponent(prefab, new Vector3(), new Quaternion(), parent);
        }
        
        public static T InstantiatePrefabForComponent<T>(T prefab, Vector3 position, Quaternion rotation, Transform parent = null) where T : UnityEngine.Object
        {
            return GameObject.Instantiate(prefab, position, rotation, parent);
        }
    }
}