using System;
using MyAudios.MyUiFramework.Utils.Soundies.Domain.Constant;
using Sources.BoundedContexts.AdvertisingAfterWaves.Infrastructure.Services;
using Sources.BoundedContexts.EnemySpawners.Presentation.Implementation;
using Sources.BoundedContexts.GameCompleted.Infrastructure.Services.Interfaces;
using Sources.BoundedContexts.GameOvers.Infrastructure.Services.Interfaces;
using Sources.BoundedContexts.RootGameObjects.Presentation;
using Sources.BoundedContexts.SaveAfterWaves.Infrastructure.Services;
using Sources.BoundedContexts.Tutorials.Services.Interfaces;
using Sources.ECSBoundedContexts.StarUps.Interfaces;
using Sources.Frameworks.DoozyWrappers.SignalBuses.Controllers.Interfaces.Collectors;
using Sources.Frameworks.GameServices.Cameras.Infrastructure.Services.Interfaces;
using Sources.Frameworks.GameServices.Curtains.Presentation.Interfaces;
using Sources.Frameworks.GameServices.Prefabs.Interfaces.Composites;
using Sources.Frameworks.GameServices.Scenes.Controllers.Interfaces;
using Sources.Frameworks.GameServices.Scenes.Domain.Interfaces;
using Sources.Frameworks.GameServices.Scenes.Infrastructure.Views.Interfaces;
using Sources.Frameworks.GameServices.UpdateServices.Interfaces;
using Sources.Frameworks.MyAudio_master.MyAudio.Soundy.Sources.Soundies.Infrastructure.Interfaces;
using Sources.Frameworks.MyGameCreator.Achievements.Infrastructure.Services.Interfaces;
using Sources.Frameworks.MyGameCreator.SkyAndWeathers.Infrastructure.Services.Implementation;
using Sources.Frameworks.UiFramework.Core.Services.Localizations.Interfaces;
using Sources.Frameworks.YandexSdkFramework.Advertisings.Services.Interfaces;
using Sources.Frameworks.YandexSdkFramework.Focuses.Interfaces;

namespace Sources.BoundedContexts.Scenes.Controllers
{
    public class GameplayScene : IScene
    {
        private readonly SaveAfterWaveService _saveAfterWaveService;
        private readonly AdvertisingAfterWaveService _advertisingAfterWaveService;
        private readonly ICompositeAssetService _compositeAssetService;
        private readonly ISkyAndWeatherService _skyAndWeatherService;
        private readonly IAchievementService _achievementService;
        private readonly ISoundyService _soundyService;
        private readonly IGameOverService _gameOverService;
        private readonly IEcsGameStartUp _ecsGameStartUp;
        private readonly ISceneViewFactory _gameplaySceneViewFactory;
        private readonly IFocusService _focusService;
        private readonly IAdvertisingService _advertisingService;
        private readonly ILocalizationService _localizationService;
        private readonly ITutorialService _tutorialService;
        private readonly IGameCompletedService _gameCompletedService;
        private readonly ICurtainView _curtainView;
        private readonly ISignalControllersCollector _signalControllersCollector;
        private readonly ICameraService _cameraService;
        private readonly IUpdateService _updateService;
        private readonly EnemySpawnerView _enemySpawnerView;

        public GameplayScene(
            RootGameObject rootGameObject,
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
            IGameCompletedService gameCompletedService,
            ICurtainView curtainView,
            ISignalControllersCollector signalControllersCollector,
            ICameraService cameraService,
            IUpdateService updateService)
        {
            _enemySpawnerView = rootGameObject.EnemySpawnerView ?? 
                                throw new ArgumentNullException(nameof(rootGameObject.EnemySpawnerView));
            _saveAfterWaveService = saveAfterWaveService ?? throw new ArgumentNullException(nameof(saveAfterWaveService));
            _advertisingAfterWaveService = advertisingAfterWaveService ?? throw new ArgumentNullException(nameof(advertisingAfterWaveService));
            _compositeAssetService = compositeAssetService ?? throw new ArgumentNullException(nameof(compositeAssetService));
            _skyAndWeatherService = skyAndWeatherService ?? throw new ArgumentNullException(nameof(skyAndWeatherService));
            _achievementService = achievementService ?? throw new ArgumentNullException(nameof(achievementService));
            _tutorialService = tutorialService ?? throw new ArgumentNullException(nameof(tutorialService));
            _soundyService = soundyService ?? throw new ArgumentNullException(nameof(soundyService));
            _gameOverService = gameOverService ?? throw new ArgumentNullException(nameof(gameOverService));
            _ecsGameStartUp = ecsGameStartUp ?? throw new ArgumentNullException(nameof(ecsGameStartUp));
            _gameplaySceneViewFactory = gameplaySceneViewFactory ?? 
                                        throw new ArgumentNullException(nameof(gameplaySceneViewFactory));
            _focusService = focusService ?? throw new ArgumentNullException(nameof(focusService));
            _advertisingService = advertisingService ?? 
                                  throw new ArgumentNullException(nameof(advertisingService));
            _localizationService = localizationService ?? 
                                   throw new ArgumentNullException(nameof(localizationService));
            _gameCompletedService = gameCompletedService ??
                                    throw new ArgumentNullException(nameof(gameCompletedService));
            _curtainView = curtainView ?? throw new ArgumentNullException(nameof(curtainView));
            _signalControllersCollector = signalControllersCollector ?? 
                                          throw new ArgumentNullException(nameof(signalControllersCollector));
            _cameraService = cameraService ?? throw new ArgumentNullException(nameof(cameraService));
            _updateService = updateService ?? throw new ArgumentNullException(nameof(updateService));
        }

        public async void Enter(object payload = null)
        {
            _focusService.Initialize();
            await _compositeAssetService.LoadAsync();
            _localizationService.Translate();
            _gameplaySceneViewFactory.Create((IScenePayload)payload);
            _advertisingService.Initialize();
            _achievementService.Initialize();
            _signalControllersCollector.Initialize();
            _soundyService.Initialize();
            _advertisingAfterWaveService.Initialize();
            _skyAndWeatherService.Initialize();
            _gameOverService.Initialize();
            _gameCompletedService.Initialize();
            _saveAfterWaveService.Initialize();
            await _curtainView.HideAsync();
            _tutorialService.Initialize();
            _enemySpawnerView.StartSpawn();
            _soundyService.PlaySequence(
                SoundyDBConst.BackgroundMusicDB, SoundyDBConst.GameplaySound);
        }

        public void Exit()
        {
            _signalControllersCollector.Destroy();
            _soundyService.StopSequence(
                SoundyDBConst.BackgroundMusicDB, SoundyDBConst.GameplaySound);
            _soundyService.Destroy();
            _skyAndWeatherService.Destroy();
            _achievementService.Destroy();
            _gameOverService.Destroy();
            _gameCompletedService.Destroy();
            _saveAfterWaveService.Destroy();
            _advertisingAfterWaveService.Destroy();
            _compositeAssetService.Release();
            _cameraService.Destroy();
        }

        public void Update(float deltaTime)
        {
            _updateService.Update(deltaTime);
        }

        public void UpdateLate(float deltaTime)
        {
        }

        public void UpdateFixed(float fixedDeltaTime)
        {
        }
    }
}