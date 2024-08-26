using System;
using System.Threading;
using Agava.WebUtility;
using Agava.YandexGames;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using Sources.BoundedContexts.HealthBoosters.Domain;
using Sources.BoundedContexts.Ids.Domain.Constant;
using Sources.BoundedContexts.PlayerWallets.Domain.Models;
using Sources.Frameworks.GameServices.Loads.Services.Interfaces;
using Sources.Frameworks.GameServices.Pauses.Services.Interfaces;
using Sources.Frameworks.YandexSdcFramework.Advertisings.Domain.Constant;
using Sources.Frameworks.YandexSdcFramework.Advertisings.Services.Interfaces;
using Sources.Frameworks.YandexSdcFramework.ServicesInterfaces.AdverticingServices;
using Sources.Frameworks.YandexSdkFramework.Advertisings.Services.Interfaces;

namespace Sources.Frameworks.YandexSdkFramework.Advertisings.Services.Implementation
{
    public class AdvertisingService : IInterstitialAdService, IVideoAdService, IAdvertisingService
    {
        private readonly IPauseService _pauseService;
        private readonly ILoadService _loadService;

        private HealthBooster _healthBooster;
        private CancellationTokenSource _cancellationTokenSource;
        private TimeSpan _timeSpan = TimeSpan.FromSeconds(35);

        public AdvertisingService(IPauseService pauseService, ILoadService loadService)
        {
            _pauseService = pauseService ?? throw new ArgumentNullException(nameof(pauseService));
            _loadService = loadService ?? throw new ArgumentNullException(nameof(loadService));
        }

        public bool IsAvailable { get; private set; } = true;

        public void Initialize() =>
            _cancellationTokenSource = new CancellationTokenSource();

        public void Destroy() =>
            _cancellationTokenSource.Cancel();

        public void Construct(HealthBooster healthBooster) =>
            _healthBooster = healthBooster ?? throw new ArgumentNullException(nameof(healthBooster));

        public void ShowInterstitial()
        {
            if (WebApplication.IsRunningOnWebGL == false)
                return;

            if (AdBlock.Enabled)
                return;

            if (IsAvailable == false)
                return;

            bool isContinue = false;
            bool isContinueSound = false;

            InterstitialAd.Show(
                () =>
                {
                    if (_pauseService.IsPaused == false)
                    {
                        isContinue = true;
                        _pauseService.Pause();
                    }

                    if (_pauseService.IsSoundPaused == false)
                    {
                        isContinueSound = true;
                        _pauseService.PauseSound();
                    }
                },
                _ =>
                {
                    if (isContinue)
                        _pauseService.Continue();

                    if (isContinueSound)
                        _pauseService.ContinueSound();
                    
                    StartTimer(_cancellationTokenSource.Token);
                });
        }

        public void ShowVideo(Action onCloseCallback)
        {
            if (WebApplication.IsRunningOnWebGL == false)
            {
                onCloseCallback?.Invoke();

                return;
            }

            if (AdBlock.Enabled)
            {
                onCloseCallback?.Invoke();

                return;
            }
            
            bool isContinue = false;
            bool isContinueSound = false;

            VideoAd.Show(
                () =>
                {
                    if (_pauseService.IsPaused == false)
                    {
                        isContinue = true;
                        _pauseService.Pause();
                    }

                    if (_pauseService.IsSoundPaused == false)
                    {
                        isContinueSound = true;
                        _pauseService.PauseSound();
                    }
                },
                () =>
                {
                    _healthBooster.Amount += HealthBoosterConst.BoosterAmount;
                    _loadService.Save(ModelId.HealthBooster);
                },
                () =>
                {
                    if (isContinue)
                        _pauseService.Continue();

                    if (isContinueSound)
                        _pauseService.ContinueSound();

                    onCloseCallback?.Invoke();
                });
        }

        private async void StartTimer(CancellationToken cancellationToken)
        {
            try
            {
                IsAvailable = false;
                await UniTask.Delay(_timeSpan, cancellationToken: cancellationToken);
                IsAvailable = true;
            }
            catch (OperationCanceledException)
            {
            }
        }
    }
}