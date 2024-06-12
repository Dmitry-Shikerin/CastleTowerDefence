using System;
using System.Collections.Generic;
using Sources.Frameworks.GameServices.ObjectPools.Implementation.Objects;
using Sources.Frameworks.GameServices.ObjectPools.Interfaces.Generic;
using Sources.Frameworks.MVPPassiveView.Presentations.Implementation.Views;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Sources.Frameworks.GameServices.ObjectPools.Implementation
{
    public class ObjectPool<T> : IObjectPool<T> 
        where T : View
    {
        private readonly Queue<T> _objects = new Queue<T>();
        private readonly List<T> _collection = new List<T>();
        private readonly Transform _parent = new GameObject($"Pool of {typeof(T).Name}").transform;

        private int _maxCount = -1;
        
        public event Action<int> ObjectCountChanged;
        
        public IReadOnlyList<T> Collection => _collection;

        public void SetPoolCount(int count)
        {
            if (count <= 0)
                throw new ArgumentOutOfRangeException(nameof(count));
            
            _maxCount = count;
        }
        
        public TType Get<TType>()
            where TType : View
        {
            if (_objects.Count == 0)
                return null;

            if (_objects.Dequeue() is not TType @object)
                return null;

            if (@object == null)
                return null;

            @object.SetParent(null);
            ObjectCountChanged?.Invoke(_objects.Count);

            return @object;
        }

        public void Return(PoolableObject poolableObject)
        {
            if (poolableObject.TryGetComponent(out T @object) == false)
                return;

            if (_objects.Contains(@object))
                throw new InvalidOperationException(nameof(@object));

            if (_maxCount != -1)
            {
                if (_collection.Count >= _maxCount)
                {
                    _collection.Remove(@object);
                    Object.Destroy(poolableObject);
                    
                    return;
                }
            }

            poolableObject.transform.SetParent(_parent);
            _objects.Enqueue(@object);
            ObjectCountChanged?.Invoke(_objects.Count);
        }

        public void PoolableObjectDestroyed(PoolableObject poolableObject)
        {
            T element = poolableObject.GetComponent<T>();
            _collection.Remove(element);
        }

        public void AddToCollection(T @object)
        {
            if (_collection.Contains(@object))
                throw new InvalidOperationException(nameof(@object));
            
            _collection.Add(@object);
        }

        public bool Contains(T @object) =>
            _objects.Contains(@object);
    }
}