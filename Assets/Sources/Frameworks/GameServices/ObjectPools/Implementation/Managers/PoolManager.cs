using System;
using System.Collections.Generic;
using Sources.Frameworks.GameServices.ObjectPools.Implementation.Objects;
using Sources.Frameworks.GameServices.ObjectPools.Interfaces;
using Sources.Frameworks.GameServices.Prefabs.Implementation;
using Sources.Frameworks.MVPPassiveView.Presentations.Implementation.Views;
using TeoGames.Mesh_Combiner.Scripts.Combine;
using Unity.VisualScripting;
using Object = UnityEngine.Object;

namespace Sources.Frameworks.GameServices.ObjectPools.Implementation.Managers
{
    public class PoolManager : IPoolManager
    {
        private Dictionary<Type, IObjectPool> _pools = new Dictionary<Type, IObjectPool>();
        private ResourcesPrefabLoader _resourcesPrefabLoader = new ResourcesPrefabLoader();
        
        public T Get<T>(string resourcesPath) where T : View
        {
            if (_pools.ContainsKey(typeof(T)) == false)
            {
                _pools[typeof(T)] = new ObjectPool<T>();
                
                return CreateObject<T>(resourcesPath);
            }

            return (T)_pools[typeof(T)].Get<T>()?.Show() ?? CreateObject<T>(resourcesPath);
        }

        private T CreateObject<T>(string resourcesPath) where T : View
        {
            T resourceObject =_resourcesPrefabLoader.Load<T>(resourcesPath);
            T gameObject = Object.Instantiate(resourceObject);
            PoolableObject poolableObject = gameObject.AddComponent<PoolableObject>();
            Combinable combinable = gameObject.AddComponent<Combinable>();
            ObjectPool<T> pool = _pools[typeof(T)] as ObjectPool<T>;
            pool.AddToCollection(gameObject);
            poolableObject.SetPool(pool);

            return gameObject;
        }        
    }
}