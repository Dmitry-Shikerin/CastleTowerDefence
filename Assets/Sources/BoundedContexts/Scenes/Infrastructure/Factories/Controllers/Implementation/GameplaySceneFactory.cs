using System;
using Cysharp.Threading.Tasks;
using Sources.App.Factories;
using Sources.App.Scenes;
using Sources.BoundedContexts.Scenes.Infrastructure.Factories.Views.Implementation;
using Sources.BoundedContexts.Scenes.Infrastructure.Factories.Views.Interfaces;
using Sources.ControllersInterfaces.Scenes;

namespace Sources.BoundedContexts.Scenes.Infrastructure.Factories.Controllers.Implementation
{
    public class GameplaySceneFactory : ISceneFactory
    {
        private readonly ISceneViewFactory _sceneViewFactory;

        public GameplaySceneFactory(
            ISceneViewFactory gameplaySceneViewFactory)
        {
            _sceneViewFactory = gameplaySceneViewFactory ?? 
                                        throw new ArgumentNullException(nameof(gameplaySceneViewFactory));
        }

        public UniTask<IScene> Create(object payload)
        {
            IScene gameplayScene = new GameplayScene(
                _sceneViewFactory);

            return UniTask.FromResult(gameplayScene);
        }
    }
}