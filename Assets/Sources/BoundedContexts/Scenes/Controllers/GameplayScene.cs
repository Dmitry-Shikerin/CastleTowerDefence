using System;
using Sources.BoundedContexts.GameOvers.Infrastructure.Services.Interfaces;
using Sources.BoundedContexts.Scenes.Infrastructure.Factories.Views.Interfaces;
using Sources.ControllersInterfaces.Scenes;
using Sources.ECSBoundedContexts.StarUps;
using Sources.ECSBoundedContexts.StarUps.Interfaces;
using Sources.Frameworks.DoozyWrappers.SignalBuses.Controllers.Interfaces;
using Sources.Frameworks.DoozyWrappers.SignalBuses.Controllers.Interfaces.Collectors;
using Sources.Frameworks.GameServices.Curtains.Presentation.Interfaces;
using Sources.Frameworks.GameServices.Scenes.Domain.Interfaces;
using Sources.Frameworks.UiFramework.AudioSources.Infrastructure.Services.AudioService.Interfaces;
using Sources.Frameworks.UiFramework.AudioSources.Presentations.Implementation.Types;
using Sources.Frameworks.UiFramework.ServicesInterfaces.Localizations;
using Sources.Frameworks.YandexSdcFramework.Advertisings.Services.Interfaces;
using Sources.Frameworks.YandexSdcFramework.Focuses.Interfaces;

namespace Sources.BoundedContexts.Scenes.Controllers
{
    public class GameplayScene : IScene
    {
        private readonly IGameOverService _gameOverService;
        private readonly IEcsGameStartUp _ecsGameStartUp;
        private readonly ISceneViewFactory _gameplaySceneViewFactory;
        private readonly IFocusService _focusService;
        private readonly IAdvertisingService _advertisingService;
        private readonly ILocalizationService _localizationService;
        private readonly IAudioService _audioService;
        private readonly ICurtainView _curtainView;
        private readonly ISignalControllersCollector _signalControllersCollector;

        public GameplayScene(
            IGameOverService gameOverService,
            IEcsGameStartUp ecsGameStartUp,
            ISceneViewFactory gameplaySceneViewFactory,
            IFocusService focusService,
            IAdvertisingService advertisingService,
            ILocalizationService localizationService,
            IAudioService audioService,
            ICurtainView curtainView,
            ISignalControllersCollector signalControllersCollector)
        {
            _gameOverService = gameOverService ?? throw new ArgumentNullException(nameof(gameOverService));
            _ecsGameStartUp = ecsGameStartUp ?? throw new ArgumentNullException(nameof(ecsGameStartUp));
            _gameplaySceneViewFactory = gameplaySceneViewFactory ?? 
                                        throw new ArgumentNullException(nameof(gameplaySceneViewFactory));
            _focusService = focusService ?? throw new ArgumentNullException(nameof(focusService));
            _advertisingService = advertisingService ?? 
                                  throw new ArgumentNullException(nameof(advertisingService));
            _localizationService = localizationService ?? 
                                   throw new ArgumentNullException(nameof(localizationService));
            _audioService = audioService ?? throw new ArgumentNullException(nameof(audioService));
            _curtainView = curtainView ?? throw new ArgumentNullException(nameof(curtainView));
            _signalControllersCollector = signalControllersCollector ?? 
                                          throw new ArgumentNullException(nameof(signalControllersCollector));
        }

        public void Enter(object payload = null)
        {
            _focusService.Initialize();
            _gameplaySceneViewFactory.Create((IScenePayload)payload);
            _advertisingService.Initialize();
            _localizationService.Translate();
            _audioService.Initialize();
            _signalControllersCollector.Initialize();
            _audioService.Play(AudioGroupId.GameplayBackground);
            _ecsGameStartUp.Initialize();
            _gameOverService.Initialize();
            // await _curtainView.HideAsync();
        }

        public void Exit()
        {
            _ecsGameStartUp.Destroy();
            _signalControllersCollector.Destroy();
            _audioService.Stop(AudioGroupId.GameplayBackground);
            _audioService.Destroy();
            _gameOverService.Destroy();
        }

        public void Update(float deltaTime)
        {
            _ecsGameStartUp.Update(deltaTime);
        }

        public void UpdateLate(float deltaTime)
        {
        }

        public void UpdateFixed(float fixedDeltaTime)
        {
        }
    }
}