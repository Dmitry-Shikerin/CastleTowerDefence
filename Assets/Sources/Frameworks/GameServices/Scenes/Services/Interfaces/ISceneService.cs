using Cysharp.Threading.Tasks;
using Sources.ControllersInterfaces.ControllerLifetimes;
using Sources.Frameworks.GameServices.UpdateServices.Interfaces.Methods;
using Sources.InfrastructureInterfaces.Services.UpdateServices.Methods;

namespace Sources.Frameworks.GameServices.Scenes.Services.Interfaces
{
    public interface ISceneService : IUpdatable, IFixedUpdatable, ILateUpdatable, IDisable
    {
        UniTask ChangeSceneAsync(string sceneName, object payload = null);
    }
}