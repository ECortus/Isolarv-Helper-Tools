using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace GameDevUtils.Runtime
{
    public readonly struct ObjectPool<T>
        where T : MonoBehaviour
    {
        readonly GameObject _owner;
        
        readonly int _preQueueCount;
        
        readonly string _id;
        readonly T[] _prefabs;
        readonly Transform _parent;
        
        readonly Queue<T> _pool;

        public string Id => _id;
        public int Count => _pool.Count;

        readonly ActionToInstant ActionToInstantFunction;
        public delegate void ActionToInstant(ref T instant, string id);

        public ObjectPool(GameObject owner, string id, T[] prefabs, Transform parent, int preQueueCount = 0, 
            ActionToInstant actionToInstant = null)
        {
            _owner = owner;
            
            _id = id;
            _prefabs = prefabs;

            var obj = new GameObject(string.Format("Pool {0}", _id));
            obj.transform.SetParent(parent);
            _parent = obj.transform;
            
            _preQueueCount = preQueueCount;

            ActionToInstantFunction = actionToInstant;
            
            _pool = new Queue<T>();
            CreatePreQueue().Forget();
        }

        async UniTaskVoid CreatePreQueue()
        {
            if (_preQueueCount > 0)
            {
                CancellationTokenSource token = new CancellationTokenSource();
                token.RegisterRaiseCancelOnDestroy(_owner.gameObject);
                
                int updatedUnitsInFrame = 0;
                
                for (int i = 0; i < _preQueueCount; i++)
                {
                    var fps = 1 / Time.deltaTime;
                    var toUpdateUnitsPerFrame = _preQueueCount / fps;
                    
                    var instance = Instantiate();
                    Enqueue(instance);
                    
                    updatedUnitsInFrame++;
                    if (updatedUnitsInFrame >= toUpdateUnitsPerFrame)
                    {
                        updatedUnitsInFrame = 0;
                        await UniTask.Yield(cancellationToken: token.Token);
                    }
                }
            }
        }

        public bool TryEnqueue(string id, T item)
        {
            if (CompareID(id))
            {
                Enqueue(item);
                return true;
            }

            return false;
        }
        
        public T TryDequeue(string id)
        {
            if (CompareID(id))
            {
                return Dequeue();
            }

            return null;
        }
        
        bool CompareID(string id)
        {
            if (_id.Equals(id))
            {
                return true;
            }

            return false;
        }

        void Enqueue(T item)
        {
            item.gameObject.SetActive(false);
            _pool.Enqueue(item);
        }

        T Dequeue()
        {
            if (_pool.Count == 0)
            {
                var instance = Instantiate();
                Enqueue(instance);
            }
            
            return _pool.Dequeue();
        }

        T Instantiate()
        {
            T prefab;
            if (_prefabs.Length > 1)
            {
                prefab = _prefabs[Random.Range(0, _prefabs.Length)];
            }
            else
            {
                prefab = _prefabs[0];
            }
            
            var instance = ObjectInstantiator.InstantiatePrefabForComponent(prefab, _parent);
            if (ActionToInstantFunction != null)
            {
                ActionToInstantFunction(ref instance, _id);
            }
            
            return instance;
        }
    }
}