using Sources.Frameworks.YandexSdcFramework.Focuses.Implementation;
using Sources.Frameworks.YandexSdcFramework.Focuses.Interfaces;
using Sources.Frameworks.YandexSdcFramework.Services.Leaderboards;
using Sources.Frameworks.YandexSdcFramework.Services.PlayerAccounts;
using Sources.Frameworks.YandexSdcFramework.Services.SdcInitializeServices;
using Sources.Frameworks.YandexSdcFramework.Services.Stickies;
using Sources.Frameworks.YandexSdcFramework.ServicesInterfaces.Leaderboads;
using Sources.Frameworks.YandexSdcFramework.ServicesInterfaces.PlayerAccounts;
using Sources.Frameworks.YandexSdcFramework.ServicesInterfaces.SdcInitializeServices;
using Sources.Infrastructure.Factories.Controllers.YandexSDK;
using Sources.Infrastructure.Factories.Views.YandexSDK;
using Sources.Infrastructure.Services.PauseServices;
using Sources.InfrastructureInterfaces.Services.PauseServices;
using Sources.InfrastructureInterfaces.Services.YandexSDKServices;
using Zenject;

namespace Sources.App.DIContainers.MainMenu
{
    public class MainMenuSdkInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindServices();
        }
        
        private void BindServices()
        {
            Container.Bind<IPauseService>().To<PauseService>().AsSingle();
            //Container.Bind<IEnemySpawnerConfigCollectionService>().To<EnemySpawnerConfigCollectionService>().AsSingle();
            //Container.Bind<IUpgradeConfigCollectionService>().To<UpgradeConfigCollectionService>().AsSingle();
            Container.Bind<ILeaderboardInitializeService>().To<YandexLeaderboardInitializeService>().AsSingle();
            Container.Bind<ILeaderBoardScoreSetter>().To<YandexLeaderBoardScoreSetter>().AsSingle();
            Container.Bind<IPlayerAccountAuthorizeService>().To<PlayerAccountAuthorizeService>().AsSingle();
            Container.Bind<ISdcInitializeService>().To<SdcInitializeService>().AsSingle();
            Container.Bind<IStickyService>().To<StickyService>().AsSingle();
            Container.Bind<IFocusService>().To<FocusService>().AsSingle();
            Container.Bind<LeaderBoardElementViewFactory>().AsSingle();
            Container.Bind<LeaderBoardElementPresenterFactory>().AsSingle();
        }

        private void BindMainMenuLoadService()
        {
            //Container.Bind<CreateMainMenuSceneService>().AsSingle();
            //Container.Bind<LoadMainMenuSceneService>().AsSingle();
        }
        
        private void BindLevelAvailability()
        {
            //Container.Bind<LevelAvailabilityPresenterFactory>().AsSingle();
            //Container.Bind<LevelAvailabilityViewFactory>().AsSingle();
        }
    }
}