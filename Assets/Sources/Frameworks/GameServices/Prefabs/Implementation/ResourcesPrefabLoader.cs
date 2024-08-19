using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Sources.Frameworks.GameServices.Prefabs.Implementation
{
    public class ResourcesPrefabLoader : IPrefabLoader
    {
        private readonly IPrefabCollector _prefabCollector;
        private List<GameObject> _gameObjects = new List<GameObject>();
        private List<Object> _objects = new List<Object>();

        public ResourcesPrefabLoader(IPrefabCollector prefabCollector)
        {
            _prefabCollector = prefabCollector ?? throw new ArgumentNullException(nameof(prefabCollector));
        }

        public async UniTask<T> LoadAsset<T>(string address) where T : MonoBehaviour
        {
            Object assetResult = await Resources
                .LoadAsync<T>(address);

            GameObject asset = assetResult.GameObject();
            
            if(asset.TryGetComponent(out T component) == false)
                throw new InvalidOperationException(nameof(component));
            
            _gameObjects.Add(asset);
            _prefabCollector.Add(typeof(T), component);
            
            return component;
        }
        
        public async UniTask<T> LoadObject<T>(string address) where T : Object
        {
            Object asset = await Resources
                .LoadAsync<T>(address);
            
            if(asset == null)
                throw new InvalidOperationException(nameof(asset));
            
            if(asset is not T component)
                throw new InvalidOperationException(typeof(T).Name);
            
            _objects.Add(asset);
            _prefabCollector.Add(typeof(T), component);
            
            return component;
        }

        public void Release()
        {
            _gameObjects.ForEach(_prefabCollector.Remove);
            _gameObjects.Clear();
            // _gameObjects.ForEach(Resources.UnloadAsset);
            _objects.ForEach(_prefabCollector.Remove);
            _objects.Clear();
            // _objects.ForEach(Resources.UnloadAsset);
        }
    }
}