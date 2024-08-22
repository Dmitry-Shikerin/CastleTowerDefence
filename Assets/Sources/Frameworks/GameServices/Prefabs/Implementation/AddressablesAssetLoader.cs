using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Sources.Frameworks.GameServices.Prefabs.Interfaces;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Sources.Frameworks.GameServices.Prefabs.Implementation
{
    public class AddressablesAssetLoader : IAddressablesAssetLoader
    {
        private readonly IAssetCollector _assetCollector;
        private readonly List<Object> _objects;

        public AddressablesAssetLoader(IAssetCollector assetCollector)
        {
            _assetCollector = assetCollector ?? throw new ArgumentNullException(nameof(assetCollector));
            _objects = new List<Object>();
        }
        
        public async UniTask<T> LoadAsset<T>(string address)
            where T : Object
        {
            T asset = await UnityEngine.AddressableAssets.Addressables.LoadAssetAsync<T>(address).Task;
            
            if(asset == null)
                throw new InvalidOperationException(nameof(asset));
            
            if(asset is not T component)
                throw new InvalidOperationException(nameof(asset));
            
            _objects.Add(asset);
            _assetCollector.Add(typeof(T), component);
            
            return component;
        }

        public void ReleaseAll()
        {
            _objects.ForEach(_assetCollector.Remove);
            _objects.ForEach(UnityEngine.AddressableAssets.Addressables.Release);
            _objects.Clear();
        }
    }
}