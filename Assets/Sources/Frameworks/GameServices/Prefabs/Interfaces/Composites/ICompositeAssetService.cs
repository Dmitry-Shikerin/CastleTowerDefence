using Cysharp.Threading.Tasks;

namespace Sources.Frameworks.GameServices.Prefabs.Interfaces.Composites
{
    public interface ICompositeAssetService
    {
        UniTask LoadAsync();
        void Release();
    }
}