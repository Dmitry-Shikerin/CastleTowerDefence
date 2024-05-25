using System;
using Sources.BoundedContexts.Scenes.Infrastructure.Factories.Views;
using Sources.ControllersInterfaces.Scenes;

namespace Sources.App.Scenes
{
    public class GameplayScene : IScene
    {
        private readonly GameplaySceneViewFactory _gameplaySceneViewFactory;

        public GameplayScene(
            GameplaySceneViewFactory gameplaySceneViewFactory)
        {
            _gameplaySceneViewFactory = gameplaySceneViewFactory ?? 
                                        throw new ArgumentNullException(nameof(gameplaySceneViewFactory));
        }

        public void Enter(object payload = null)
        {
            _gameplaySceneViewFactory.Create();
        }

        public void Exit()
        {
        }

        public void Update(float deltaTime)
        {
        }

        public void UpdateLate(float deltaTime)
        {
        }

        public void UpdateFixed(float fixedDeltaTime)
        {
        }
    }
}