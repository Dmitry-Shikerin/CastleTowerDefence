using Cysharp.Threading.Tasks;
using Sources.App.Factories;
using Sources.App.Scenes;
using Sources.ControllersInterfaces.Scenes;

namespace Sources.BoundedContexts.Scenes.Infrastructure.Factories.Controllers
{
    public class MainMenuSceneFactory : ISceneFactory
    {
        public UniTask<IScene> Create(object payload)
        {
            IScene mainMenuScene = new MainMenuScene();
            
            return UniTask.FromResult(mainMenuScene);
        }
    }
}