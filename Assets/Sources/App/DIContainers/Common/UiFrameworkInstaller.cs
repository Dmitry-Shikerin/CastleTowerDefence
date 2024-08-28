using Sirenix.OdinInspector;
using Sources.Domain.Models.Constants;
using Sources.Frameworks.DoozyWrappers.SignalBuses.Controllers.Implementation;
using Sources.Frameworks.DoozyWrappers.SignalBuses.Controllers.Implementation.Collectors;
using Sources.Frameworks.DoozyWrappers.SignalBuses.Controllers.Interfaces.Collectors;
using Sources.Frameworks.DoozyWrappers.SignalBuses.Infrastructure.ButtonCommands.Implementation;
using Sources.Frameworks.DoozyWrappers.SignalBuses.Infrastructure.ViewCommands.Implementation;
using Sources.Frameworks.MyAudio_master.MyAudio.Soundy.Sources.Soundies.Infrastructure.Implementation;
using Sources.Frameworks.MyAudio_master.MyAudio.Soundy.Sources.Soundies.Infrastructure.Interfaces;
using Sources.Frameworks.UiFramework.Collectors;
using Sources.Frameworks.UiFramework.Core.Services.Forms.Implementation;
using Sources.Frameworks.UiFramework.Core.Services.Localizations.Implementation;
using Sources.Frameworks.UiFramework.Core.Services.Localizations.Interfaces;
using Sources.Frameworks.UiFramework.Texts.Services.Localizations.Configs;
using Sources.Frameworks.UiFramework.Views.Presentations.Implementation;
using UnityEngine;
using Zenject;

namespace Sources.App.DIContainers.Common
{
    public class UiFrameworkInstaller : MonoInstaller
    {
        [Required] [SerializeField] private UiCollector _uiCollector;
        
        public override void InstallBindings()
        {
            Container.Bind<UiCollector>().FromInstance(_uiCollector);
            Container.Bind<UiCollectorFactory>().AsSingle();
            Container.BindInterfacesAndSelfTo<FormService>().AsSingle();
            Container.Bind<ILocalizationService>().To<LocalizationService>().AsSingle();
            Container
                .Bind<LocalizationDataBase>()
                .FromResources(LocalizationConst.LocalizationDataBaseAssetPath);
            
            //Soundy
            Container.Bind<ISoundyService>().To<SoundyService>().AsSingle();

            //SignalControllers
            Container.Bind<ISignalControllersCollector>().To<SignalControllerCollector>().AsSingle();
            Container.Bind<ButtonsCommandSignalController>().AsSingle();
            Container.Bind<ViewCommandSignalController>().AsSingle();
            
            //Buttons
            Container.Bind<UnPauseButtonCommand>().AsSingle();
            Container.Bind<PauseButtonCommand>().AsSingle();
            Container.Bind<ShowRewardedAdvertisingButtonCommand>().AsSingle();
            Container.Bind<NewGameCommand>().AsSingle();
            Container.Bind<LoadGameCommand>().AsSingle();
            Container.Bind<ShowLeaderboardCommand>().AsSingle().NonLazy();
            Container.Bind<CompleteTutorialCommand>().AsSingle();
            Container.Bind<LoadMainMenuSceneCommand>().AsSingle();
            Container.Bind<ClearSavesButtonCommand>().AsSingle();
            Container.Bind<SaveVolumeButtonCommand>().AsSingle();
            Container.Bind<ShowDailyRewardViewCommand>().AsSingle();
            Container.Bind<PlayerAccountAuthorizeButtonCommand>().AsSingle();

            //Views
            Container.Bind<UnPauseCommand>().AsSingle();
            Container.Bind<PauseCommand>().AsSingle();
            Container.Bind<SaveVolumeCommand>().AsSingle();
            Container.Bind<ClearSavesCommand>().AsSingle();
        }
    }
}