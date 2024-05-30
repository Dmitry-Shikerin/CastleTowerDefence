using Cysharp.Threading.Tasks;
using Sources.ControllersInterfaces.Scenes;

namespace Sources.BoundedContexts.Scenes.Infrastructure.Factories.Controllers.Interfaces
{
    public interface ISceneFactory
    {
        UniTask<IScene> Create(object payload);
    }
}