using System;
using System.Collections.Generic;
using Sources.BoundedContexts.Enemies.Presentation;
using Sources.Frameworks.GameServices.ObjectPools.Implementation.Objects;
using Sources.Frameworks.GameServices.ObjectPools.Interfaces;
using Sources.Frameworks.GameServices.ObjectPools.Interfaces.Generic;
using Sources.Frameworks.GameServices.Prefabs.Implementation;
using Sources.Frameworks.MVPPassiveView.Presentations.Implementation.Views;
using Sources.Frameworks.MVPPassiveView.Presentations.Interfaces.PresentationsInterfaces.Views;
using TeoGames.Mesh_Combiner.Scripts.Combine;
using Unity.VisualScripting;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Sources.Frameworks.GameServices.ObjectPools.Implementation.Managers
{
    public class PoolManager : IPoolManager
    {
        private readonly Transform _root = new GameObject("Root of Pools").transform;
        private readonly Dictionary<Type, IObjectPool> _pools = new Dictionary<Type, IObjectPool>();
        private readonly ResourcesPrefabLoader _resourcesPrefabLoader = new ResourcesPrefabLoader();

        public T Get<T>(string resourcesPath) where T : View
        {
            if (_pools.ContainsKey(typeof(T)) == false)
            {
                _pools[typeof(T)] = new ObjectPool<T>();
                
                return CreateObject<T>(resourcesPath);
            }

            return (T)_pools[typeof(T)].Get<T>()?.Show() ?? CreateObject<T>(resourcesPath);
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

        private T CreateObject<T>(string resourcesPath) where T : View
        {
            T resourceObject =_resourcesPrefabLoader.Load<T>(resourcesPath);
            T gameObject = Object.Instantiate(resourceObject);
            PoolableObject poolableObject = gameObject.AddComponent<PoolableObject>();
            ObjectPool<T> pool = _pools[typeof(T)] as ObjectPool<T>;
            pool.PoolBaker.Add(gameObject);
            pool.PoolBaker.SetRootParent(_root);
            pool.SetRootParent(_root);
            pool.AddToCollection(gameObject);
            poolableObject.SetPool(pool);

            return gameObject;
        }        
    }
}