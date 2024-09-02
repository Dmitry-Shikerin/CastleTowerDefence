using Cysharp.Threading.Tasks;
using Sirenix.Utilities;
using Sources.Frameworks.GameServices.Prefabs.Interfaces;
using Object = UnityEngine.Object;

namespace Sources.Frameworks.GameServices.Prefabs.Implementation
{
    public class AddressablesAssetLoader : AssetLoaderBase, IAddressablesAssetLoader
    {
        public AddressablesAssetLoader(
            IAssetCollector assetCollector) 
            : base(assetCollector)
        {
        }
        
        protected override async UniTask<Object> LoadAssetAsync<T>(string address) =>
            await UnityEngine.AddressableAssets.Addressables.LoadAssetAsync<T>(address).Task;

        public override void ReleaseAll()
        {
            Objects.ForEach(UnityEngine.AddressableAssets.Addressables.Release);
            base.ReleaseAll();
        }
    }
}