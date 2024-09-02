using Sources.Frameworks.GameServices.ServerTimes.Services;
using Sources.Frameworks.GameServices.ServerTimes.Services.Implementation;
using Sources.Frameworks.GameServices.ServerTimes.Services.Interfaces;
using Sources.Frameworks.YandexSdkFramework.Advertisings.Services.Implementation;
using Sources.Frameworks.YandexSdkFramework.Infrastructure.Factories.Controllers;
using Sources.Frameworks.YandexSdkFramework.Infrastructure.Factories.Views;
using Sources.Frameworks.YandexSdkFramework.Leaderboards.Services.Implementation;
using Sources.Frameworks.YandexSdkFramework.Leaderboards.Services.Interfaces;
using Sources.Frameworks.YandexSdkFramework.PlayerAccounts.Implementation;
using Sources.Frameworks.YandexSdkFramework.PlayerAccounts.Interfaces;
using Sources.Frameworks.YandexSdkFramework.SdcInitializes.Implementation;
using Sources.Frameworks.YandexSdkFramework.SdcInitializes.Interfaces;
using Sources.Frameworks.YandexSdkFramework.Stickies.Implementation;
using Sources.Frameworks.YandexSdkFramework.Stickies.Interfaces;
using Zenject;

namespace Sources.App.DIContainers.Common
{
    public class SdkServicesInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<ILeaderboardInitializeService>().To<YandexLeaderboardInitializeService>().AsSingle();
            Container.Bind<ILeaderBoardScoreSetter>().To<YandexLeaderBoardScoreSetter>().AsSingle();
            Container.Bind<IPlayerAccountAuthorizeService>().To<PlayerAccountAuthorizeService>().AsSingle();
            Container.Bind<ISdkInitializeService>().To<SdkInitializeService>().AsSingle();
            Container.Bind<IStickyService>().To<StickyService>().AsSingle();
            Container.BindInterfacesTo<AdvertisingService>().AsSingle();
            Container.Bind<LeaderBoardElementViewFactory>().AsSingle();
            Container.Bind<LeaderBoardElementPresenterFactory>().AsSingle();
            //Todo раскоментировать после релиза
            // Container.Bind<ITimeService>().To<NetworkTimeService>().AsSingle();
            Container.Bind<ITimeService>().To<DayTimeService>().AsSingle();
        }
    }
}