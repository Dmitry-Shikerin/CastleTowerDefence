using Sirenix.OdinInspector;
using Sources.BoundedContexts.SignalCollectors.Controllers;
using Sources.Domain.Models.Constants;
using Sources.Frameworks.UiFramework.AudioSources.Domain.Configs;
using Sources.Frameworks.UiFramework.AudioSources.Infrastructure.Services.AudioService.Implementation;
using Sources.Frameworks.UiFramework.AudioSources.Infrastructure.Services.AudioService.Interfaces;
using Sources.Frameworks.UiFramework.ButtonCommands.Implementation;
using Sources.Frameworks.UiFramework.ButtonProviders.Infrastructure.Commands.Implementation;
using Sources.Frameworks.UiFramework.ButtonProviders.Infrastructure.Services.Interfaces;
using Sources.Frameworks.UiFramework.Collectors;
using Sources.Frameworks.UiFramework.Core.Services.Forms.Implementation;
using Sources.Frameworks.UiFramework.Core.Services.Localizations.Implementation;
using Sources.Frameworks.UiFramework.Infrastructure.Commands.Buttons;
using Sources.Frameworks.UiFramework.Infrastructure.Commands.Forms;
using Sources.Frameworks.UiFramework.Services.Buttons;
using Sources.Frameworks.UiFramework.ServicesInterfaces.Localizations;
using Sources.Frameworks.UiFramework.Texts.Services.Localizations.Configs;
using Sources.Frameworks.UiFramework.Views.Commands.Implementation;
using Sources.Frameworks.UiFramework.Views.Presentations.Implementation;
using Sources.Frameworks.YandexSdcFramework.Leaderboards.Controllers.Actions;
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
            Container.Bind<IUiButtonService>().To<UiButtonService>().AsSingle();
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
            
            //Buttons
            Container.Bind<MainMenuButtonsCommandSignalController>().AsSingle();
            Container.Bind<ShowLeaderboardSignalAction>().AsSingle();
            
            Container.Bind<UnPauseButtonCommand>().AsSingle();
            Container.Bind<ShowRewardedAdvertisingButtonCommand>().AsSingle();
            Container.Bind<NewGameCommand>().AsSingle();
            Container.Bind<ShowLeaderboardCommand>().AsSingle();
            Container.Bind<CompleteTutorialCommand>().AsSingle();
            Container.Bind<LoadMainMenuSceneCommand>().AsSingle();
            Container.Bind<ClearSavesButtonCommand>().AsSingle();
            
            //Views
            Container.Bind<UnPauseCommand>().AsSingle();
            Container.Bind<PauseCommand>().AsSingle();
            Container.Bind<SaveVolumeCommand>().AsSingle();
            Container.Bind<ClearSavesCommand>().AsSingle();
        }
    }
}