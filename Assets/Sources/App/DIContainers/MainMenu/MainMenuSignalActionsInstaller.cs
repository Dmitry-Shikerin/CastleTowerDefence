using Sources.BoundedContexts.SignalCollectors.Controllers;
using Sources.BoundedContexts.SignalCollectors.Infrastructure.Factories;
using Sources.Frameworks.GameServices.DoozySignalBuses.Controllers.Interfaces;
using Sources.Frameworks.YandexSdcFramework.Leaderboards.Controllers.Actions;
using Zenject;

namespace Sources.App.DIContainers.MainMenu
{
    public class MainMenuSignalActionsInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<ISignalControllersCollector>()
                .To<MainMenuSignalControllerCollector>()
                .AsSingle();
            Container.Bind<ButtonSignalController>().AsSingle();
            Container.Bind<ShowLeaderboardSignalAction>().AsSingle();
        }
    }
}