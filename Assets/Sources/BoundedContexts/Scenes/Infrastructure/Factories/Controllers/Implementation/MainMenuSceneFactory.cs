using System;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using Sources.App.Factories;
using Sources.App.Scenes;
using Sources.ControllersInterfaces.Scenes;

namespace Sources.BoundedContexts.Scenes.Infrastructure.Factories.Controllers
{
    public class MainMenuSceneFactory : ISceneFactory
    {
        private readonly MainMenuSceneViewFactory _mainMenuSceneViewFactory;

        public MainMenuSceneFactory(MainMenuSceneViewFactory mainMenuSceneViewFactory)
        {
            _mainMenuSceneViewFactory = mainMenuSceneViewFactory ??
                                        throw new ArgumentNullException(nameof(mainMenuSceneViewFactory));
        }
        
        public UniTask<IScene> Create(object payload)
        {
            IScene mainMenuScene = new MainMenuScene(_mainMenuSceneViewFactory);
            
            return UniTask.FromResult(mainMenuScene);
        }
    }
}