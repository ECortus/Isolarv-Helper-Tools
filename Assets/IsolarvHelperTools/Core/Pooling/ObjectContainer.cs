using System.Collections.Generic;
using UnityEngine;

namespace IsolarvHelperTools
{
    public readonly struct ObjectContainer<T> where T : MonoBehaviour
    {
        private readonly string _mainId;
        private readonly Queue<ObjectPool<T>> _queue;

        private readonly GameObject _owner;
        private readonly Transform _containerParent;

        private readonly int _preQueueCount;
        private readonly ObjectPool<T>.ActionToInstant _actionToInstant;
        
        public ObjectContainer(string mainId, GameObject owner, Transform parent, 
            int preQueueCount = 0, ObjectPool<T>.ActionToInstant actionToInstant = null)
        {
            _mainId = mainId;
            _owner = owner;
            
            _containerParent = new GameObject(string.Format("Container {0}", _mainId)).transform;
            _containerParent.SetParent(parent);
            
            _preQueueCount = preQueueCount;
            _actionToInstant = actionToInstant;
            
            _queue = new Queue<ObjectPool<T>>();
        }

        public ObjectPool<T> CreateNewPool(string id, T[] prefabs) 
        {
            var pool = new ObjectPool<T>(_owner, id, prefabs, _containerParent, _preQueueCount, _actionToInstant);
            _queue.Enqueue(pool);
            return pool;
        }

        public bool HasSimilarPool(string id)
        {
            foreach (var q in _queue)
            {
                if (q.Id.Equals(id))
                {
                    return true;
                }
            }

            return false;
        }
        
        public bool TryEnqueue(string id, T element)
        {
            foreach (var pool in _queue)
            {
                if (pool.TryEnqueue(id, element))
                {
                    return true;
                }
            }

            Debug.LogError(string.Format("ID {0} cannot found created pool, creating pool automatically in {1} container.", id, _mainId));
            
            CreateNewPool(id, new [] { element });
            return TryEnqueue(id, element);
        }

        public T TryDequeue(string id, T element)
        {
            foreach (var pool in _queue)
            {
                var item = pool.TryDequeue(id);
                if (item)
                {
                    return item;
                }
            }
            
            Debug.LogError(string.Format("ID {0} cannot found created pool, creating pool automatically in {1} container.", id, _mainId));
            
            CreateNewPool(id, new [] { element });
            return TryDequeue(id, element);
        }
        
        public T TryDequeue(string id, T[] elements)
        {
            foreach (var pool in _queue)
            {
                var item = pool.TryDequeue(id);
                if (item)
                {
                    return item;
                }
            }
            
            Debug.LogError(string.Format("ID {0} cannot found created pool, creating pool automatically in {1} container.", id, _mainId));
            
            CreateNewPool(id, elements);
            return TryDequeue(id, elements);
        }
    }
}