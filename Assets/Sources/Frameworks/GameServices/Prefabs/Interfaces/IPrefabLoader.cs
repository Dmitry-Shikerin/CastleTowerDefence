using UnityEngine;

namespace Sources.Frameworks.GameServices.Prefabs.Implementation
{
    public interface IPrefabLoader
    {
        T Load<T>(string path) 
            where T : Object;
    }
}