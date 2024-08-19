using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Sources.Frameworks.GameServices.Prefabs.Implementation
{
    public interface IPrefabLoader
    {
        UniTask<T> LoadAsset<T>(string address) 
            where T : MonoBehaviour;
        UniTask<T> LoadObject<T>(string address) 
            where T : Object;
        void Release();
    }
}