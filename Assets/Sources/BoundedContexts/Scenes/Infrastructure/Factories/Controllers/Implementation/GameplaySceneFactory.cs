using System;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using MyAudios.MyUiFramework.Utils.Soundies.Infrastructure;
using Sources.BoundedContexts.AdvertisingAfterWaves.Infrrastructure.Services;
using Sources.BoundedContexts.GameOvers.Infrastructure.Services.Interfaces;
using Sources.BoundedContexts.SaveAfterWaves.Infrastructure.Services;
using Sources.BoundedContexts.Scenes.Controllers;
using Sources.BoundedContexts.Scenes.Infrastructure.Factories.Controllers.Interfaces;
using Sources.BoundedContexts.Scenes.Infrastructure.Factories.Views.Interfaces;
using Sources.BoundedContexts.Tutorials.Services.Interfaces;
using Sources.ControllersInterfaces.Scenes;
using Sources.ECSBoundedContexts.StarUps.Interfaces;
using Sources.Frameworks.DoozyWrappers.SignalBuses.Controllers.Interfaces.Collectors;
using Sources.Frameworks.GameServices.Curtains.Presentation.Interfaces;
using Sources.Frameworks.GameServices.Prefabs.Interfaces.Composites;
using Sources.Frameworks.MyGameCreator.Achivements.Infrastructure.Services.Interfaces;
using Sources.Frameworks.MyGameCreator.SkyAndWeathers.Infrastructure.Services.Implementation;
using Sources.Frameworks.UiFramework.ServicesInterfaces.Localizations;
using Sources.Frameworks.YandexSdcFramework.Advertisings.Services.Interfaces;
using Sources.Frameworks.YandexSdcFramework.Focuses.Interfaces;

namespace Sources.BoundedContexts.Scenes.Infrastructure.Factories.Controllers.Implementation
{
    public class GameplaySceneFactory : ISceneFactory
    {
        private readonly SaveAfterWaveService _saveAfterWaveService;
        private readonly AdvertisingAfterWaveService _advertisingAfterWaveService;
        private readonly ICompositeAssetService _compositeAssetService;
        private readonly ISkyAndWeatherService _skyAndWeatherService;
        private readonly IAchievementService _achievementService;
        private readonly ISoundyService _soundyService;
        private readonly IGameOverService _gameOverService;
        private readonly IEcsGameStartUp _ecsGameStartUp;
        private readonly ISceneViewFactory _sceneViewFactory;
        private readonly IFocusService _focusService;
        private readonly IAdvertisingService _advertisingService;
        private readonly ILocalizationService _localizationService;
        private readonly ITutorialService _tutorialService;
        private readonly ICurtainView _curtainView;
        private readonly ISignalControllersCollector _signalControllersCollector;

        public GameplaySceneFactory(
            SaveAfterWaveService saveAfterWaveService,
            AdvertisingAfterWaveService advertisingAfterWaveService,
            ICompositeAssetService compositeAssetService,
            ISkyAndWeatherService skyAndWeatherService,
            IAchievementService achievementService,
            ISoundyService soundyService,
            IGameOverService gameOverService,
            IEcsGameStartUp ecsGameStartUp,
            ISceneViewFactory gameplaySceneViewFactory,
            IFocusService focusService,
            IAdvertisingService advertisingService,
            ILocalizationService localizationService,
            ITutorialService tutorialService,
            ICurtainView curtainView,
            ISignalControllersCollector signalControllersCollector)
        {
            _saveAfterWaveService = saveAfterWaveService ?? throw new ArgumentNullException(nameof(saveAfterWaveService));
            _advertisingAfterWaveService = advertisingAfterWaveService ?? 
                                           throw new ArgumentNullException(nameof(advertisingAfterWaveService));
            _compositeAssetService = compositeAssetService ?? 
                                     throw new ArgumentNullException(nameof(compositeAssetService));
            _skyAndWeatherService = skyAndWeatherService ?? throw new ArgumentNullException(nameof(skyAndWeatherService));
            _achievementService = achievementService ?? throw new ArgumentNullException(nameof(achievementService));
            _tutorialService = tutorialService ?? throw new ArgumentNullException(nameof(tutorialService));
            _soundyService = soundyService ?? throw new ArgumentNullException(nameof(soundyService));
            _gameOverService = gameOverService ?? throw new ArgumentNullException(nameof(gameOverService));
            _ecsGameStartUp = ecsGameStartUp ?? throw new ArgumentNullException(nameof(ecsGameStartUp));
            _sceneViewFactory = gameplaySceneViewFactory ?? 
                                throw new ArgumentNullException(nameof(gameplaySceneViewFactory));
            _focusService = focusService ?? throw new ArgumentNullException(nameof(focusService));
            _advertisingService = advertisingService ?? throw new ArgumentNullException(nameof(advertisingService));
            _localizationService = localizationService ?? throw new ArgumentNullException(nameof(localizationService));
            _curtainView = curtainView ?? throw new ArgumentNullException(nameof(curtainView));
            _signalControllersCollector = signalControllersCollector ?? 
                                          throw new ArgumentNullException(nameof(signalControllersCollector));
        }

        public UniTask<IScene> Create(object payload)
        {
            IScene gameplayScene = new GameplayScene(
                _saveAfterWaveService,
                _advertisingAfterWaveService,
                _compositeAssetService,
                _skyAndWeatherService,
                _achievementService,
                _soundyService,
                _gameOverService,
                _ecsGameStartUp,
                _sceneViewFactory,
                _focusService,
                _advertisingService,
                _localizationService,
                _tutorialService,
                _curtainView,
                _signalControllersCollector);

            return UniTask.FromResult(gameplayScene);
        }
    }
}