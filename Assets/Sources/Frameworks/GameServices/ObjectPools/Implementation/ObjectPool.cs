using System;
using System.Collections.Generic;
using Sources.Frameworks.GameServices.ObjectPools.Implementation.Bakers;
using Sources.Frameworks.GameServices.ObjectPools.Implementation.Managers;
using Sources.Frameworks.GameServices.ObjectPools.Implementation.Objects;
using Sources.Frameworks.GameServices.ObjectPools.Interfaces.Generic;
using Sources.Frameworks.GameServices.Prefabs.Implementation;
using Sources.Frameworks.MVPPassiveView.Presentations.Implementation.Views;
using Unity.VisualScripting;
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
        private readonly IPoolBaker<T> _poolBaker;
        private readonly ResourcesPrefabLoader _resourcesPrefabLoader;
        private readonly Transform _root;

        private int _maxCount = -1;
        private bool _isInitialized;
        private PoolManagerConfig _config;
        private string _resourcesPath;

        public ObjectPool(
            ResourcesPrefabLoader resourcesPrefabLoader,
            Transform parent = null,
            PoolManagerConfig poolManagerConfig = null,
            string resourcesPath = null)
        {
            _resourcesPrefabLoader = resourcesPrefabLoader ??
                                     throw new ArgumentNullException(nameof(resourcesPrefabLoader));
            _root = parent;
            _parent.SetParent(parent);
            _poolBaker = new PoolBaker<T>(_root);
            _config = poolManagerConfig;
            
            if (_config != null)
                _maxCount = _config.MaxPoolCount;
            
            _resourcesPath = resourcesPath;

            if (_config != null && _config.IsWarmUp)
            {
                for (int i = 0; i < _config.WarmUpCount; i++)
                {
                    CreateObject(_resourcesPath)
                        .GetComponent<PoolableObject>()
                        .ReturnToPool();
                    Debug.Log(i);
                }
            }
        }

        public event Action<int> ObjectCountChanged;
        public IPoolBaker<T> PoolBaker => _poolBaker;
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
                return CreateObject(_resourcesPath) as TType;

            if (_objects.Dequeue() is not TType @object)
                return null;

            if (@object == null)
                return null;

            // @object.SetParent(null);
            _poolBaker.Add(@object);
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
            poolableObject.Hide();
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

        private T CreateObject(string resourcesPath)
        {
            T resourceObject = _resourcesPrefabLoader.Load<T>(resourcesPath);
            T gameObject = Object.Instantiate(resourceObject);
            PoolableObject poolableObject = gameObject.AddComponent<PoolableObject>();
            PoolBaker.Add(gameObject);
            AddToCollection(gameObject);
            poolableObject.SetPool(this);

            return gameObject;
        }

        public bool Contains(T @object) =>
            _objects.Contains(@object);
    }
}