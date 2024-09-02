using Cysharp.Threading.Tasks;
using Sources.Frameworks.GameServices.Scenes.Controllers.Interfaces;

namespace Sources.Frameworks.GameServices.Scenes.Infrastructure.Factories.Controllers.Interfaces
{
    public interface ISceneFactory
    {
        UniTask<IScene> Create(object payload);
    }
}