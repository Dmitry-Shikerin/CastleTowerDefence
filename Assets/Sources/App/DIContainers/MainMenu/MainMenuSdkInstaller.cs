﻿using Sources.Frameworks.YandexSdcFramework.Services.Leaderboards;
using Sources.Frameworks.YandexSdcFramework.Services.PlayerAccounts;
using Sources.Frameworks.YandexSdcFramework.Services.SdcInitializeServices;
using Sources.Frameworks.YandexSdcFramework.Services.Stickies;
using Sources.Frameworks.YandexSdcFramework.ServicesInterfaces.Leaderboads;
using Sources.Frameworks.YandexSdcFramework.ServicesInterfaces.PlayerAccounts;
using Sources.Frameworks.YandexSdcFramework.ServicesInterfaces.SdcInitializeServices;
using Sources.Infrastructure.Factories.Controllers.YandexSDK;
using Sources.Infrastructure.Factories.Views.YandexSDK;
using Sources.InfrastructureInterfaces.Services.YandexSDKServices;
using Zenject;

namespace Sources.App.DIContainers.MainMenu
{
    public class MainMenuSdkInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<ILeaderboardInitializeService>().To<YandexLeaderboardInitializeService>().AsSingle();
            Container.Bind<ILeaderBoardScoreSetter>().To<YandexLeaderBoardScoreSetter>().AsSingle();
            Container.Bind<IPlayerAccountAuthorizeService>().To<PlayerAccountAuthorizeService>().AsSingle();
            Container.Bind<ISdcInitializeService>().To<SdcInitializeService>().AsSingle();
            Container.Bind<IStickyService>().To<StickyService>().AsSingle();
            Container.Bind<LeaderBoardElementViewFactory>().AsSingle();
            Container.Bind<LeaderBoardElementPresenterFactory>().AsSingle();
        }
    }
}