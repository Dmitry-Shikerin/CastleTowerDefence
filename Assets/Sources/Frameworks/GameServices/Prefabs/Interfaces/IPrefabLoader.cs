using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Sources.Frameworks.GameServices.Prefabs.Interfaces
{
    public interface IPrefabLoader
    {
        UniTask<T> LoadAsset<T>(string address) 
            where T : Object;
        void ReleaseAll();
    }
}