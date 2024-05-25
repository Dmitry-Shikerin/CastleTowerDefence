using System;
using System.Collections.Generic;
using Sources.Presentations.Views;
using UnityEngine;

namespace Sources.Frameworks.Services.ObjectPools.Implementation
{
    public class ObjectPool<T> : IObjectPool
    {
        private readonly Queue<T> _objects = new Queue<T>();
        private readonly Transform _parent = new GameObject($"Pool of {typeof(T).Name}").transform;
        
        public event Action<int> ObjectCountChanged;

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

            poolableObject.transform.SetParent(_parent);
            _objects.Enqueue(@object);
            ObjectCountChanged?.Invoke(_objects.Count);
        }
    }
}