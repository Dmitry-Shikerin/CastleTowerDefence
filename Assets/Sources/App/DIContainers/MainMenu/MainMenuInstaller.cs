using Sirenix.OdinInspector;
using Sources.BoundedContexts.Huds.Presentations;
using Sources.BoundedContexts.Scenes.Infrastructure.Factories.Controllers.Implementation;
using Sources.BoundedContexts.Scenes.Infrastructure.Factories.Controllers.Interfaces;
using Sources.BoundedContexts.Scenes.Infrastructure.Factories.Views.Implementation;
using Sources.BoundedContexts.Scenes.Infrastructure.Factories.Views.Interfaces;
using Sources.BoundedContexts.SignalCollectors.Infrastructure.Factories;
using Sources.Frameworks.GameServices.DoozySignalBuses.Controllers.Interfaces;
using Sources.Frameworks.UiFramework.ButtonCommands.Implementation.Handlers;
using Sources.Frameworks.UiFramework.ButtonCommands.Interfaces.Handlers;
using UnityEngine;
using Zenject;

namespace Sources.App.DIContainers.MainMenu
{
    public class MainMenuInstaller : MonoInstaller
    {
        [Required] [SerializeField] private MainMenuHud _mainMenuHud;
        
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<MainMenuHud>().FromInstance(_mainMenuHud).AsSingle();
            Container.Bind<ISceneViewFactory>().To<MainMenuSceneViewFactory>().AsSingle();
            Container.Bind<ISceneFactory>().To<MainMenuSceneFactory>().AsSingle();
            
            //CommandHandlers
            Container.Bind<IButtonCommandHandler>().To<MainMenuButtonCommandHandler>().AsSingle();
            
            //SignalControllers
            Container.Bind<ISignalControllersCollector>()
                .To<MainMenuSignalControllerCollector>()
                .AsSingle();
        }
    }
}