using System;
using Cysharp.Threading.Tasks;
using Sources.App.Factories;
using Sources.App.Scenes;
using Sources.BoundedContexts.Scenes.Infrastructure.Factories.Views;
using Sources.ControllersInterfaces.Scenes;

namespace Sources.BoundedContexts.Scenes.Infrastructure.Factories.Controllers
{
    public class GameplaySceneFactory : ISceneFactory
    {
        private readonly GameplaySceneViewFactory _gameplaySceneViewFactory;

        public GameplaySceneFactory(
            GameplaySceneViewFactory gameplaySceneViewFactory)
        {
            _gameplaySceneViewFactory = gameplaySceneViewFactory ?? 
                                        throw new ArgumentNullException(nameof(gameplaySceneViewFactory));
        }

        public UniTask<IScene> Create(object payload)
        {
            IScene gameplayScene = new GameplayScene(
                _gameplaySceneViewFactory);

            return UniTask.FromResult(gameplayScene);
        }
    }
}