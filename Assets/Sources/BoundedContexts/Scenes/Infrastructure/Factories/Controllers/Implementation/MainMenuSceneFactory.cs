using System;
using Cysharp.Threading.Tasks;
using Sources.App.Factories;
using Sources.BoundedContexts.Scenes.Controllers;
using Sources.BoundedContexts.Scenes.Infrastructure.Factories.Controllers.Interfaces;
using Sources.BoundedContexts.Scenes.Infrastructure.Factories.Views.Interfaces;
using Sources.ControllersInterfaces.Scenes;
using Sources.Frameworks.GameServices.DoozySignalBuses.Controllers.Interfaces;
using UnityEngine;

namespace Sources.BoundedContexts.Scenes.Infrastructure.Factories.Controllers.Implementation
{
    public class MainMenuSceneFactory : ISceneFactory
    {
        private readonly ISceneViewFactory _sceneViewFactory;
        private readonly ISignalControllersCollector _signalControllersCollector;

        public MainMenuSceneFactory(
            ISceneViewFactory sceneViewFactory,
            ISignalControllersCollector signalControllersCollector)
        {
            _sceneViewFactory = sceneViewFactory ??
                                        throw new ArgumentNullException(nameof(sceneViewFactory));
            _signalControllersCollector = signalControllersCollector ?? 
                                          throw new ArgumentNullException(nameof(signalControllersCollector));
        }
        
        public UniTask<IScene> Create(object payload)
        {
            IScene mainMenuScene = new MainMenuScene(
                _sceneViewFactory,
                _signalControllersCollector);
            
            return UniTask.FromResult(mainMenuScene);
        }
    }
}