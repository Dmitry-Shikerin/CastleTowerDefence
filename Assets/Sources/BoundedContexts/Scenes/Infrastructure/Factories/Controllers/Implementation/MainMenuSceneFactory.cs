using System;
using Cysharp.Threading.Tasks;
using Sources.BoundedContexts.Scenes.Controllers;
using Sources.BoundedContexts.Scenes.Infrastructure.Factories.Controllers.Interfaces;
using Sources.BoundedContexts.Scenes.Infrastructure.Factories.Views.Interfaces;
using Sources.ControllersInterfaces.Scenes;
using Sources.Frameworks.DoozyWrappers.SignalBuses.Controllers.Interfaces;
using Sources.Frameworks.DoozyWrappers.SignalBuses.Controllers.Interfaces.Collectors;
using Sources.Frameworks.GameServices.Curtains.Presentation.Interfaces;
using Sources.Frameworks.UiFramework.AudioSources.Infrastructure.Services.AudioService.Interfaces;
using Sources.Frameworks.UiFramework.ServicesInterfaces.Localizations;
using Sources.Frameworks.YandexSdcFramework.Focuses.Interfaces;
using Sources.Frameworks.YandexSdcFramework.Services.Stickies;
using Sources.Frameworks.YandexSdcFramework.ServicesInterfaces.SdcInitializeServices;

namespace Sources.BoundedContexts.Scenes.Infrastructure.Factories.Controllers.Implementation
{
    public class MainMenuSceneFactory : ISceneFactory
    {
        private readonly ISceneViewFactory _sceneViewFactory;
        private readonly ISignalControllersCollector _signalControllersCollector;
        private readonly ISdkInitializeService _sdkInitializeService;
        private readonly IFocusService _focusService;
        private readonly ILocalizationService _localizationService;
        private readonly IAudioService _audioService;
        private readonly ICurtainView _curtainView;
        private readonly IStickyService _stickyService;

        public MainMenuSceneFactory(
            ISceneViewFactory sceneViewFactory,
            ISignalControllersCollector signalControllersCollector,
            ISdkInitializeService sdkInitializeService,
            IFocusService focusService,
            ILocalizationService localizationService,
            IAudioService audioService,
            ICurtainView curtainView,
            IStickyService stickyService)
        {
            _sceneViewFactory = sceneViewFactory ??
                                        throw new ArgumentNullException(nameof(sceneViewFactory));
            _signalControllersCollector = signalControllersCollector ?? 
                                          throw new ArgumentNullException(nameof(signalControllersCollector));
            _sdkInitializeService = sdkInitializeService ?? 
                                    throw new ArgumentNullException(nameof(sdkInitializeService));
            _focusService = focusService ?? throw new ArgumentNullException(nameof(focusService));
            _localizationService = localizationService ?? 
                                   throw new ArgumentNullException(nameof(localizationService));
            _audioService = audioService ?? throw new ArgumentNullException(nameof(audioService));
            _curtainView = curtainView ?? throw new ArgumentNullException(nameof(curtainView));
            _stickyService = stickyService ?? throw new ArgumentNullException(nameof(stickyService));
        }
        
        public UniTask<IScene> Create(object payload)
        {
            IScene mainMenuScene = new MainMenuScene(
                _sceneViewFactory,
                _signalControllersCollector,
                _sdkInitializeService,
                _focusService,
                _localizationService,
                _audioService,
                _curtainView,
                _stickyService);
            
            return UniTask.FromResult(mainMenuScene);
        }
    }
}