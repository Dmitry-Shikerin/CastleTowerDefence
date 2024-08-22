using System;
using System.Collections.Generic;
using Object = UnityEngine.Object;

namespace Sources.Frameworks.GameServices.Prefabs.Interfaces
{
    public interface IAssetCollector
    {
        IReadOnlyDictionary<Type, Object> Prefabs { get; }
        
        void Add(Type type, Object prefab);
        void Remove(Type type);
        void Remove(Object prefab);
        T Get<T>() 
            where T : Object;
    }
}