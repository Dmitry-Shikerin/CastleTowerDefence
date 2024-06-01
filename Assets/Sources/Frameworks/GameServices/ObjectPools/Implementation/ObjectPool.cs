using System;
using System.Collections.Generic;
using Sources.Frameworks.GameServices.ObjectPools.Implementation.Objects;
using Sources.Frameworks.GameServices.ObjectPools.Interfaces.Generic;
using Sources.Frameworks.MVPPassiveView.Presentations.Implementation.Views;
using Sources.Frameworks.MVPPassiveView.Presentations.Interfaces.PresentationsInterfaces.Views;
using Sources.PresentationsInterfaces.Views;
using UnityEngine;

namespace Sources.Frameworks.GameServices.ObjectPools.Implementation
{
    public class ObjectPool<T> : IObjectPool<T> 
        where T : IView
    {
        private readonly Queue<T> _objects = new Queue<T>();
        private readonly List<T> _collection = new List<T>();
        private readonly Transform _parent = new GameObject($"Pool of {typeof(T).Name}").transform;
        
        public event Action<int> ObjectCountChanged;
        public IReadOnlyList<T> Collection => _collection;

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

        public void AddToCollection(T @object)
        {
            if(_objects.Contains(@object))
                return;
            
            _collection.Add(@object);
        }

        public void RemoveFromCollection(T @object)
        {
            if (_objects.Contains(@object) == false)
                return;
            
            _collection.Remove(@object);
        }
    }
}