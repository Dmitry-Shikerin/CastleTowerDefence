using System;
using Sources.BoundedContexts.Scenes.Infrastructure.Factories.Views;
using Sources.BoundedContexts.Scenes.Infrastructure.Factories.Views.Implementation;
using Sources.BoundedContexts.Scenes.Infrastructure.Factories.Views.Interfaces;
using Sources.ControllersInterfaces.Scenes;
using Sources.Frameworks.GameServices.Scenes.Domain.Interfaces;

namespace Sources.App.Scenes
{
    public class GameplayScene : IScene
    {
        private readonly ISceneViewFactory _gameplaySceneViewFactory;

        public GameplayScene(
            ISceneViewFactory gameplaySceneViewFactory)
        {
            _gameplaySceneViewFactory = gameplaySceneViewFactory ?? 
                                        throw new ArgumentNullException(nameof(gameplaySceneViewFactory));
        }

        public void Enter(object payload = null)
        {
            _gameplaySceneViewFactory.Create((IScenePayload)payload);
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