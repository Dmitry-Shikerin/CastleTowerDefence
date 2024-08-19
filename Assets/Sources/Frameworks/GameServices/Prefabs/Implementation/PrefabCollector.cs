using System;
using System.Collections.Generic;
using Object = UnityEngine.Object;

namespace Sources.Frameworks.GameServices.Prefabs.Implementation
{
    public class PrefabCollector : IPrefabCollector
    {
        private readonly Dictionary<Type, Object> _prefabs = new Dictionary<Type, Object>();
        
        public IReadOnlyDictionary<Type, Object> Prefabs => _prefabs;
        
        public void Add(Type type, Object prefab) =>
            _prefabs.Add(type, prefab);

        public void Remove(Type type) =>
            _prefabs.Remove(type);

        public void Remove(Object prefab) =>
            _prefabs.Remove(prefab.GetType());

        public T Get<T>() where T : Object
        {
            if (_prefabs.ContainsKey(typeof(T)) == false)
                throw new KeyNotFoundException(typeof(T).Name);

            if (_prefabs[typeof(T)] is not T concrete)
                throw new InvalidCastException(typeof(T).Name);
            
            return (T)_prefabs[typeof(T)];
        }
    }
}