using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Sources.Frameworks.GameServices.Addressables.Interfaces;
using Sources.Frameworks.GameServices.Prefabs.Implementation;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Sources.Frameworks.GameServices.Addressables.Implementation
{
    public class AddressablesAssetLoader : IAssetProvider
    {
        private readonly IPrefabCollector _prefabCollector;
        private List<GameObject> _gameObjects = new List<GameObject>();
        private List<Object> _objects = new List<Object>();

        public AddressablesAssetLoader(IPrefabCollector prefabCollector)
        {
            _prefabCollector = prefabCollector ?? throw new ArgumentNullException(nameof(prefabCollector));
        }

        public async UniTask<T> LoadAsset<T>(string address) where T : MonoBehaviour
        {
            GameObject asset = await UnityEngine.AddressableAssets.Addressables
                .LoadAssetAsync<GameObject>(address).Task;
            
            if(asset.TryGetComponent(out T component) == false)
                throw new InvalidOperationException(nameof(component));
            
            _gameObjects.Add(asset);
            _prefabCollector.Add(typeof(T), component);
            
            return component;
        }
        
        public async UniTask<T> LoadObject<T>(string address) where T : Object
        {
            Object asset = await UnityEngine.AddressableAssets.Addressables
                .LoadAssetAsync<Object>(address).Task;
            
            if(asset == null)
                throw new InvalidOperationException(nameof(asset));
            
            if(asset is not T component)
                throw new InvalidOperationException(nameof(asset));
            
            _objects.Add(asset);
            _prefabCollector.Add(typeof(T), component);
            
            return component;
        }

        public void Release()
        {
            _gameObjects.ForEach(_prefabCollector.Remove);
            _gameObjects.ForEach(UnityEngine.AddressableAssets.Addressables.Release);
            _objects.ForEach(_prefabCollector.Remove);
            _objects.ForEach(UnityEngine.AddressableAssets.Addressables.Release);
        }
    }
}