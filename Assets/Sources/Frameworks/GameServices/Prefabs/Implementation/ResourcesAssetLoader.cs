using Cysharp.Threading.Tasks;
using Sources.Frameworks.GameServices.Prefabs.Interfaces;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Sources.Frameworks.GameServices.Prefabs.Implementation
{
    public class ResourcesAssetLoader : AssetLoaderBase, IResourcesAssetLoader
    {
        public ResourcesAssetLoader(
            IAssetCollector assetCollector) 
            : base(assetCollector)
        {
        }
        
        protected override async UniTask<Object> LoadAssetAsync<T>(string address) =>
            await Resources.LoadAsync<T>(address);
    }
}