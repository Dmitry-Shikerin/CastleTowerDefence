using Sirenix.OdinInspector;
using Sources.BoundedContexts.RootGameObjects.Presentation;
using Sources.BoundedContexts.Scenes.Infrastructure.Factories.Controllers.Implementation;
using Sources.BoundedContexts.Scenes.Infrastructure.Factories.Controllers.Interfaces;
using Sources.BoundedContexts.Scenes.Infrastructure.Factories.Views.Implementation;
using Sources.BoundedContexts.Scenes.Infrastructure.Factories.Views.Interfaces;
using Sources.BoundedContexts.SignalCollectors.Controllers;
using Sources.BoundedContexts.SignalCollectors.Infrastructure.Factories;
using Sources.Frameworks.GameServices.DoozySignalBuses.Controllers.Interfaces;
using Sources.Frameworks.UiFramework.ButtonCommands.Implementation.Handlers;
using Sources.Frameworks.UiFramework.ButtonCommands.Interfaces.Handlers;
using Sources.Frameworks.UiFramework.Infrastructure.Commands.Forms.Handlers;
using Sources.Frameworks.UiFramework.InfrastructureInterfaces.Commands.Views.Handlers;
using UnityEngine;
using Zenject;

namespace Sources.App.DIContainers.Gameplay
{
    public class GameplayInstaller : MonoInstaller
    {
        [Required] [SerializeField] private RootGameObject _rootGameObject;
        
        public override void InstallBindings()
        {
            Container.Bind<RootGameObject>().FromInstance(_rootGameObject);
            
            Container.Bind<ISceneFactory>().To<GameplaySceneFactory>().AsSingle();
            Container.Bind<ISceneViewFactory>().To<GameplaySceneViewFactory>().AsSingle();
            
            //SignalControllers
            Container.Bind<ISignalControllersCollector>().To<GameplaySignalControllerCollector>().AsSingle();
            Container.Bind<GameplayButtonsCommandSignalController>().AsSingle();
            
            //CommandHandlers
            Container.Bind<IButtonCommandHandler>().To<GameplayButtonCommandHandler>().AsSingle();
            Container.Bind<IUiViewCommandHandler>().To<GameplayUiViewCommandHandler>().AsSingle();
        }
    }
}