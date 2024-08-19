using Cysharp.Threading.Tasks;

namespace Sources.Frameworks.GameServices.Addressables.Interfaces
{
    public interface ICompositeAssetService
    {
        UniTask LoadAsync();
        void Release();
    }
}