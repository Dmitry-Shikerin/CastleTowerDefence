using System;
using Cysharp.Threading.Tasks;
using Sources.App.Scenes;
using Sources.BoundedContexts.Scenes.Infrastructure.Factories.Views;
using Sources.ControllersInterfaces.Scenes;

namespace Sources.App.Factories
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

        public async UniTask<IScene> Create(object payload)
        {
            return new GameplayScene(_gameplaySceneViewFactory);
        }
    }
}