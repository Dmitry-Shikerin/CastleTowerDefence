using System;
using Cysharp.Threading.Tasks;
using Sources.BoundedContexts.Scenes.Infrastructure.Factories.Views.Interfaces;
using Sources.ControllersInterfaces.Scenes;
using Sources.Frameworks.DoozyWrappers.SignalBuses.Controllers.Interfaces;
using Sources.Frameworks.DoozyWrappers.SignalBuses.Controllers.Interfaces.Collectors;
using Sources.Frameworks.GameServices.Curtains.Presentation.Interfaces;
using Sources.Frameworks.GameServices.Scenes.Domain.Interfaces;
using Sources.Frameworks.UiFramework.AudioSources.Infrastructure.Services.AudioService.Interfaces;
using Sources.Frameworks.UiFramework.ServicesInterfaces.Localizations;
using Sources.Frameworks.YandexSdcFramework.Focuses.Interfaces;
using Sources.Frameworks.YandexSdcFramework.Services.Stickies;
using Sources.Frameworks.YandexSdcFramework.ServicesInterfaces.SdcInitializeServices;

namespace Sources.BoundedContexts.Scenes.Controllers
{
    public class MainMenuScene : IScene
    {
        private readonly ISceneViewFactory _sceneViewFactory;
        private readonly ISignalControllersCollector _signalControllersCollector;
        private readonly ISdkInitializeService _sdkInitializeService;
        private readonly IFocusService _focusService;
        private readonly ILocalizationService _localizationService;
        private readonly IAudioService _audioService;
        private readonly ICurtainView _curtainView;
        private readonly IStickyService _stickyService;

        public MainMenuScene(
            ISceneViewFactory mainMenuSceneViewFactory,
            ISignalControllersCollector signalControllersCollector,
            ISdkInitializeService sdkInitializeService,
            IFocusService focusService,
            ILocalizationService localizationService,
            IAudioService audioService,
            ICurtainView curtainView,
            IStickyService stickyService)
        {
            _sceneViewFactory = mainMenuSceneViewFactory ??
                                        throw new ArgumentNullException(nameof(mainMenuSceneViewFactory));
            _signalControllersCollector = signalControllersCollector ?? 
                                          throw new ArgumentNullException(nameof(signalControllersCollector));
            _sdkInitializeService = sdkInitializeService ?? 
                                    throw new ArgumentNullException(nameof(sdkInitializeService));
            _focusService = focusService ?? throw new ArgumentNullException(nameof(focusService));
            _localizationService = localizationService ?? throw new ArgumentNullException(nameof(localizationService));
            _audioService = audioService ?? throw new ArgumentNullException(nameof(audioService));
            _curtainView = curtainView ?? throw new ArgumentNullException(nameof(curtainView));
            _stickyService = stickyService ?? throw new ArgumentNullException(nameof(stickyService));
        }

        public async void Enter(object payload = null)
        {
            await InitializeAsync((IScenePayload)payload);
            _focusService.Initialize();
            _sceneViewFactory.Create(null);
            _signalControllersCollector.Initialize();
            _localizationService.Translate();
            _audioService.Initialize();
            // await _curtainView.HideAsync();
            await GameReady((IScenePayload)payload);
        }

        public void Exit()
        {
            _signalControllersCollector.Destroy();
            _audioService.Destroy();
            _focusService.Destroy();
        }

        public void Update(float deltaTime)
        {
        }

        public void UpdateLate(float deltaTime)
        {
        }

        public void UpdateFixed(float fixedDeltaTime)
        {
        }

        private async UniTask InitializeAsync(IScenePayload payload)
        {
            if (payload.CanFromGameplay)
                return;
            
            _sdkInitializeService.EnableCallbackLogging();
            await _sdkInitializeService.Initialize();
        }

        private UniTask GameReady(IScenePayload payload)
        {
            if (payload.CanFromGameplay)
                return UniTask.CompletedTask;

            _stickyService.ShowSticky();
            _sdkInitializeService.GameReady();

            return UniTask.CompletedTask;
        }
    }
}