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
using Sources.Frameworks.YandexSdcFramework.Advertisings.Services.Interfaces;
using Sources.Frameworks.YandexSdcFramework.Focuses.Interfaces;

namespace Sources.BoundedContexts.Scenes.Infrastructure.Factories.Controllers.Implementation
{
    public class GameplaySceneFactory : ISceneFactory
    {
        private readonly ISceneViewFactory _sceneViewFactory;
        private readonly IFocusService _focusService;
        private readonly IAdvertisingService _advertisingService;
        private readonly ILocalizationService _localizationService;
        private readonly IAudioService _audioService;
        private readonly ICurtainView _curtainView;
        private readonly ISignalControllersCollector _signalControllersCollector;

        public GameplaySceneFactory(
            ISceneViewFactory gameplaySceneViewFactory,
            IFocusService focusService,
            IAdvertisingService advertisingService,
            ILocalizationService localizationService,
            IAudioService audioService,
            ICurtainView curtainView,
            ISignalControllersCollector signalControllersCollector)
        {
            _sceneViewFactory = gameplaySceneViewFactory ?? 
                                throw new ArgumentNullException(nameof(gameplaySceneViewFactory));
            _focusService = focusService ?? throw new ArgumentNullException(nameof(focusService));
            _advertisingService = advertisingService ?? throw new ArgumentNullException(nameof(advertisingService));
            _localizationService = localizationService ?? throw new ArgumentNullException(nameof(localizationService));
            _audioService = audioService ?? throw new ArgumentNullException(nameof(audioService));
            _curtainView = curtainView ?? throw new ArgumentNullException(nameof(curtainView));
            _signalControllersCollector = signalControllersCollector ?? 
                                          throw new ArgumentNullException(nameof(signalControllersCollector));
        }

        public UniTask<IScene> Create(object payload)
        {
            IScene gameplayScene = new GameplayScene(
                _sceneViewFactory,
                _focusService,
                _advertisingService,
                _localizationService,
                _audioService,
                _curtainView,
                _signalControllersCollector);

            return UniTask.FromResult(gameplayScene);
        }
    }
}