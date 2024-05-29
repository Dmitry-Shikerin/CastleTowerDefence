using Cysharp.Threading.Tasks;
using Sources.ControllersInterfaces.Scenes;

namespace Sources.App.Factories
{
    public interface ISceneFactory
    {
        UniTask<IScene> Create(object payload);
    }
}