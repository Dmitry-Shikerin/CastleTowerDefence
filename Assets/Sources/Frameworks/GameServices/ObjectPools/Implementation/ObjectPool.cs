using System;
using System.Collections.Generic;
using System.Linq;
using Sources.Frameworks.GameServices.ObjectPools.Implementation.Bakers;
using Sources.Frameworks.GameServices.ObjectPools.Implementation.Managers;
using Sources.Frameworks.GameServices.ObjectPools.Implementation.Objects;
using Sources.Frameworks.GameServices.ObjectPools.Interfaces.Bakers.Generic;
using Sources.Frameworks.GameServices.ObjectPools.Interfaces.Generic;
using Sources.Frameworks.GameServices.Prefabs.Interfaces;
using Sources.Frameworks.MVPPassiveView.Presentations.Implementation.Views;
using Unity.VisualScripting;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Sources.Frameworks.GameServices.ObjectPools.Implementation
{
    public class ObjectPool<T> : IObjectPool<T>
        where T : View
    {
        private readonly List<T> _objects = new List<T>();
        private readonly List<T> _collection = new List<T>();
        private readonly Transform _parent = new GameObject($"Pool of {typeof(T).Name}").transform;
        private readonly IPoolBaker<T> _poolBaker;
        private readonly IAssetCollector _assetCollector;
        private readonly Transform _root;

        private int _maxCount = -1;
        private bool _isCountLimit;
        private bool _isInitialized;
        private PoolManagerConfig _config;

        public ObjectPool(
            IAssetCollector assetCollector,
            Transform parent = null,
            PoolManagerConfig poolManagerConfig = null)
        {
            _assetCollector = assetCollector ??
                                     throw new ArgumentNullException(nameof(assetCollector));
            _root = parent;
            _parent.SetParent(parent);
            _poolBaker = new PoolBaker<T>(_root);
            _config = poolManagerConfig;

            if (_config != null)
            {
                _maxCount = _config.MaxPoolCount;
                _isCountLimit = _config.IsCountLimit;
            }
            
            DeleteAfterTime = _config?.DeleteAfterTime ?? 0;

            if (_config != null && _config.IsWarmUp)
            {
                for (int i = 0; i < _config.WarmUpCount; i++)
                {
                    CreateObject()
                        .GetComponent<PoolableObject>()
                        .ReturnToPool();
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
                return CreateObject() as TType;

            if (_objects.FirstOrDefault() is not TType @object)
                return null;

            if (@object == null)
                return CreateObject() as TType;

            _objects.Remove(_objects.First());
            @object.GetComponent<PoolableObject>().Cancel();
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
            
            if (_isCountLimit)
            {
                if (_objects.Count >= _maxCount)
                {
                    _collection.Remove(@object);
                    poolableObject.StartTimer();
                    poolableObject.transform.SetParent(_parent);
                    _objects.Add(@object);
                    poolableObject.Hide();
                    ObjectCountChanged?.Invoke(_objects.Count);

                    return;
                }
            }

            poolableObject.transform.SetParent(_parent);
            _objects.Add(@object);
            poolableObject.Hide();
            ObjectCountChanged?.Invoke(_objects.Count);
        }

        public float DeleteAfterTime { get; }

        public void RemoveFromPool(PoolableObject poolableObject)
        {
            if (_maxCount > _objects.Count)
                return;
            
            if (poolableObject.TryGetComponent(out T @object) == false)
                return;
            
            _collection.Remove(@object);
            _objects.Remove(@object);
            Object.Destroy(poolableObject.gameObject);
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

        private T CreateObject()
        {
            T resourceObject = _assetCollector.Get<T>();
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