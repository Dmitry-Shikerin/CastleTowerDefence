using Cysharp.Threading.Tasks;
using Sources.ControllersInterfaces.ControllerLifetimes;
using Sources.InfrastructureInterfaces.Services.UpdateServices.Methods;

namespace Sources.InfrastructureInterfaces.Services.SceneServices
{
    public interface ISceneService : IUpdatable, IFixedUpdatable, ILateUpdatable, IDisable
    {
        UniTask ChangeSceneAsync(string sceneName, object payload = null);
    }
}