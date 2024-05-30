using System;
using Sources.BoundedContexts.Scenes.Infrastructure.Factories.Views.Interfaces;
using Sources.ControllersInterfaces.Scenes;
using Sources.Frameworks.GameServices.DoozySignalBuses.Controllers.Interfaces;
using Sources.Frameworks.GameServices.Scenes.Domain.Interfaces;
using UnityEngine;

namespace Sources.BoundedContexts.Scenes.Controllers
{
    public class MainMenuScene : IScene
    {
        private readonly ISceneViewFactory _sceneViewFactory;
        private readonly ISignalControllersCollector _signalControllersCollector;

        public MainMenuScene(
            ISceneViewFactory mainMenuSceneViewFactory,
            ISignalControllersCollector signalControllersCollector)
        {
            _sceneViewFactory = mainMenuSceneViewFactory ??
                                        throw new ArgumentNullException(nameof(mainMenuSceneViewFactory));
            _signalControllersCollector = signalControllersCollector ?? 
                                          throw new ArgumentNullException(nameof(signalControllersCollector));
        }

        public void Enter(object payload = null)
        {
            _sceneViewFactory.Create(null);
            _signalControllersCollector.Initialize();
        }

        public void Exit()
        {
            _signalControllersCollector.Destroy();
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