using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Sources.Frameworks.GameServices.Prefabs.Implementation
{
    public class ResourcesPrefabLoader : IPrefabLoader
    {
        private readonly Dictionary<Type, Object> _prefabs = new Dictionary<Type, Object>();
        
        public T Load<T>(string path) where T : Object
        {
            if (_prefabs.ContainsKey(typeof(T)))
                return (T)_prefabs[typeof(T)];
            
            T prefab = Resources.Load<T>(path);
            _prefabs.Add(typeof(T), prefab);
            
            return prefab;
        }
    }
}