using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Sources.Frameworks.GameServices.Prefabs.Interfaces;
using Unity.VisualScripting;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Sources.Frameworks.GameServices.Prefabs.Implementation
{
    public class ResourcesAssetLoader : IResourcesAssetLoader
    {
        private readonly IAssetCollector _assetCollector;
        private readonly List<Object> _objects;

        public ResourcesAssetLoader(IAssetCollector assetCollector)
        {
            _assetCollector = assetCollector ?? throw new ArgumentNullException(nameof(assetCollector));
            _objects = new List<Object>();
        }
        
        public async UniTask<T> LoadAsset<T>(string address) where T : Object
        {
            Object asset = await Resources.LoadAsync<T>(address);
            
            if(asset == null)
                throw new InvalidOperationException(nameof(asset));
            
            if(asset is not T component)
                throw new InvalidOperationException(typeof(T).Name);
            
            _objects.Add(asset);
            _assetCollector.Add(typeof(T), component);
            
            return component;
        }

        public void ReleaseAll()
        {
            _objects.ForEach(_assetCollector.Remove);
            _objects.Clear();
        }
    }
}