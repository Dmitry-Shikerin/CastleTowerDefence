using Cysharp.Threading.Tasks;

namespace Sources.InfrastructureInterfaces.Services.SceneLoaderService
{
    public interface ISceneLoaderService
    {
        UniTask LoadSceneAsync(string sceneName);
    }
}