using Sirenix.OdinInspector;
using Sources.BoundedContexts.Huds.Presentations;
using Sources.BoundedContexts.Scenes.Infrastructure.Factories.Controllers.Implementation;
using Sources.BoundedContexts.Scenes.Infrastructure.Factories.Domain.Implementation;
using Sources.BoundedContexts.Scenes.Infrastructure.Factories.Views.Implementation;
using Sources.Frameworks.DoozyWrappers.SignalBuses.Infrastructure.ButtonCommands.Implementation.Handlers;
using Sources.Frameworks.DoozyWrappers.SignalBuses.Infrastructure.ButtonCommands.Interfaces.Handlers;
using Sources.Frameworks.DoozyWrappers.SignalBuses.Infrastructure.ViewCommands.Implementation.Handlers;
using Sources.Frameworks.DoozyWrappers.SignalBuses.Infrastructure.ViewCommands.Interfaces.Handlers;
using Sources.Frameworks.GameServices.DailyRewards.Infrastructure.Factories;
using Sources.Frameworks.GameServices.Scenes.Infrastructure.Factories.Controllers.Interfaces;
using Sources.Frameworks.GameServices.Scenes.Infrastructure.Views.Interfaces;
using Sources.Frameworks.MyGameCreator.Achievements.Presentation;
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
            
            //ModelsLoader
            Container.Bind<MainMenuModelsCreatorService>().AsSingle();
            Container.Bind<MainMenuModelsLoaderService>().AsSingle();
            
            //UiCommands
            Container.Bind<IButtonCommandHandler>().To<MainMenuButtonCommandHandler>().AsSingle();
            Container.Bind<IViewCommandHandler>().To<MainMenuViewCommandHandler>().AsSingle();
            
            //daily
            Container.Bind<DailyRewardViewFactory>().AsSingle();
            
            //achievements
            Container.Bind<AchievementView>().FromInstance(_mainMenuHud.EmptyAchievementView).AsSingle();
        }
    }
}