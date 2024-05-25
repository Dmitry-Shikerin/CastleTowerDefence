using Cysharp.Threading.Tasks;
using Sources.App.Scenes;
using Sources.ControllersInterfaces.Scenes;

namespace Sources.App.Factories
{
    public class MainMenuSceneFactory : ISceneFactory
    {
        public async UniTask<IScene> Create(object payload)
        {
            return new MainMenuScene();
        }
    }
}