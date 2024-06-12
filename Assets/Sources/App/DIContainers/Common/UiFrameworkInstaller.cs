using Sirenix.OdinInspector;
using Sources.Domain.Models.Constants;
using Sources.Frameworks.DoozyWrappers.SignalBuses.Controllers.Implementation;
using Sources.Frameworks.DoozyWrappers.SignalBuses.Controllers.Implementation.Collectors;
using Sources.Frameworks.DoozyWrappers.SignalBuses.Controllers.Interfaces;
using Sources.Frameworks.DoozyWrappers.SignalBuses.Controllers.Interfaces.Collectors;
using Sources.Frameworks.DoozyWrappers.SignalBuses.Infrastructure.ButtonCommands.Implementation;
using Sources.Frameworks.DoozyWrappers.SignalBuses.Infrastructure.ButtonCommands.Implementation.Handlers;
using Sources.Frameworks.DoozyWrappers.SignalBuses.Infrastructure.ButtonCommands.Interfaces.Handlers;
using Sources.Frameworks.DoozyWrappers.SignalBuses.Infrastructure.ViewCommands.Implementation;
using Sources.Frameworks.DoozyWrappers.SignalBuses.Infrastructure.ViewCommands.Implementation.Handlers;
using Sources.Frameworks.DoozyWrappers.SignalBuses.Infrastructure.ViewCommands.Interfaces.Handlers;
using Sources.Frameworks.UiFramework.AudioSources.Domain.Configs;
using Sources.Frameworks.UiFramework.AudioSources.Infrastructure.Services.AudioService.Implementation;
using Sources.Frameworks.UiFramework.AudioSources.Infrastructure.Services.AudioService.Interfaces;
using Sources.Frameworks.UiFramework.Collectors;
using Sources.Frameworks.UiFramework.Core.Services.Forms.Implementation;
using Sources.Frameworks.UiFramework.Core.Services.Localizations.Implementation;
using Sources.Frameworks.UiFramework.ServicesInterfaces.Localizations;
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
            Container.Bind<IAudioService>().To<AudioService>().AsSingle();
            Container
                .Bind<AudioServiceDataBase>()
                .FromResources("Services/AudioServices/AudioServiceDataBase");
            Container.Bind<ILocalizationService>().To<LocalizationService>().AsSingle();
            Container
                .Bind<LocalizationDataBase>()
                .FromResources(LocalizationConst.LocalizationDataBaseAssetPath);
            
            //Audio
            Container.Bind<AudioServiceSignalController>().AsSingle();

            //SignalControllers
            Container.Bind<ISignalControllersCollector>().To<SignalControllerCollector>().AsSingle();
            Container.Bind<ButtonsCommandSignalController>().AsSingle();

            //Buttons
            Container.Bind<UnPauseButtonCommand>().AsSingle();
            Container.Bind<ShowRewardedAdvertisingButtonCommand>().AsSingle();
            Container.Bind<NewGameCommand>().AsSingle();
            Container.Bind<ShowLeaderboardCommand>().AsSingle().NonLazy();
            Container.Bind<CompleteTutorialCommand>().AsSingle();
            Container.Bind<LoadMainMenuSceneCommand>().AsSingle();
            Container.Bind<ClearSavesButtonCommand>().AsSingle();

            //Views
            Container.Bind<UnPauseCommand>().AsSingle();
            Container.Bind<PauseCommand>().AsSingle();
            Container.Bind<SaveVolumeCommand>().AsSingle();
            Container.Bind<ClearSavesCommand>().AsSingle();
            
            //CommandHandlers
            Container.Bind<IButtonCommandHandler>().To<ButtonCommandHandler>().AsSingle();
            Container.Bind<IUiViewCommandHandler>().To<GameplayUiViewCommandHandler>().AsSingle();
        }
    }
}