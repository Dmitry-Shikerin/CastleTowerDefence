using System;
using System.Collections.Generic;
using System.Linq;
using Sources.Frameworks.GameServices.ObjectPools.Implementation.Objects;
using Sources.Frameworks.GameServices.ObjectPools.Interfaces;
using Sources.Frameworks.GameServices.ObjectPools.Interfaces.Generic;
using Sources.Frameworks.GameServices.ObjectPools.Interfaces.Managers;
using Sources.Frameworks.GameServices.Prefabs.Implementation;
using Sources.Frameworks.MVPPassiveView.Presentations.Implementation.Views;
using Sources.Frameworks.MVPPassiveView.Presentations.Interfaces.PresentationsInterfaces.Views;
using Unity.VisualScripting;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Sources.Frameworks.GameServices.ObjectPools.Implementation.Managers
{
    public class PoolManager : IPoolManager
    {
        private readonly Transform _root = new GameObject("Root of Pools").transform;
        private readonly Dictionary<Type, IObjectPool> _pools = new Dictionary<Type, IObjectPool>();
        private readonly IPrefabLoader _prefabLoader;
        private readonly PoolManagerCollector _poolManagerCollector;

        public PoolManager(IPrefabLoader prefabLoader)
        {
            _prefabLoader = prefabLoader ?? throw new ArgumentNullException(nameof(prefabLoader));
            _poolManagerCollector = _prefabLoader.Load<PoolManagerCollector>(
                "Services/PoolManagers/PoolManagerCollector");
        }

        public T Get<T>(string resourcesPath) where T : View
        {
            if (_pools.ContainsKey(typeof(T)) == false)
            {
                PoolManagerConfig config = _poolManagerCollector.Configs
                    .FirstOrDefault(config => config.Type == typeof(T));
                _pools[typeof(T)] = new ObjectPool<T>(
                    _prefabLoader, _root, config, resourcesPath);
            }

            return (T)_pools[typeof(T)].Get<T>()?.Show();
        }

        public IObjectPool<T> GetPool<T>() where T : IView
        {
            if (_pools.ContainsKey(typeof(T)) == false)
                throw new NullReferenceException();
            
            return _pools[typeof(T)] as IObjectPool<T>;
        }

        public bool Contains<T>(T @object) 
            where T : View =>
            (_pools[typeof(T)] as IObjectPool<T>).Contains(@object);
    }
}