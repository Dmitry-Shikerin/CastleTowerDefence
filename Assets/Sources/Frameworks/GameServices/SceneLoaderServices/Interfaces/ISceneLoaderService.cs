using Cysharp.Threading.Tasks;

namespace Sources.InfrastructureInterfaces.Services.SceneLoaderService
{
    public interface ISceneLoaderService
    {
        UniTask Load(string sceneName);
        UniTask Unload();
    }
}