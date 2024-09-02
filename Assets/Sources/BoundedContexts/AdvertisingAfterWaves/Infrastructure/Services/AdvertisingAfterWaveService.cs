using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Sources.BoundedContexts.AdvertisingAfterWaves.Presentation;
using Sources.BoundedContexts.EnemySpawners.Domain.Models;
using Sources.Frameworks.GameServices.Loads.Domain.Constant;
using Sources.Frameworks.GameServices.Repositories.Services.Interfaces;
using Sources.Frameworks.MVPPassiveView.Controllers.Interfaces.ControllerLifetimes;
using Sources.Frameworks.YandexSdkFramework.Advertisings.Domain.Constant;
using Sources.Frameworks.YandexSdkFramework.Advertisings.Services.Interfaces;

namespace Sources.BoundedContexts.AdvertisingAfterWaves.Infrastructure.Services
{
    public class AdvertisingAfterWaveService : IInitialize, IDestroy
    {
        private const int WavesCount = 20;
        private const int SecondsCount = 3;
        
        private readonly IEntityRepository _entityRepository;
        private readonly IInterstitialAdService _interstitialAdService;
        private readonly AdvertisingAfterWaveView _advertisingView;
        
        private EnemySpawner _enemySpawner;
        private CancellationTokenSource _cancellationTokenSource;
        private TimeSpan _timerTimeSpan = TimeSpan.FromSeconds(AdvertisingConst.Delay);

        public AdvertisingAfterWaveService(
            IEntityRepository entityRepository, 
            IInterstitialAdService interstitialAdService,
            AdvertisingAfterWaveView advertisingView)
        {
            _entityRepository = entityRepository ?? throw new ArgumentNullException(nameof(entityRepository));
            _interstitialAdService = interstitialAdService ?? 
                                  throw new ArgumentNullException(nameof(interstitialAdService));
            _advertisingView = advertisingView ?? throw new ArgumentNullException(nameof(advertisingView));
        }

        public void Initialize()
        {
            _enemySpawner = _entityRepository.Get<EnemySpawner>(ModelId.EnemySpawner);
            _enemySpawner.WaveChanged += OnShowInterstitial;
            _cancellationTokenSource = new CancellationTokenSource();
            
            _advertisingView.Hide();
        }

        public void Destroy()
        {
            _enemySpawner.WaveChanged -= OnShowInterstitial;
            _cancellationTokenSource.Cancel();
        }

        private async void OnShowInterstitial()
        {
            if (CheckShow() == false)
                return;

            await ShowTimerAsync(_cancellationTokenSource.Token);
            
            _interstitialAdService.ShowInterstitial();
        }

        private bool CheckShow()
        {
            int waves = WavesCount;
            
            while (waves <= _enemySpawner.CurrentWaveNumber)
            {
                if (_enemySpawner.CurrentWaveNumber % waves == 0)
                    return true;

                waves += WavesCount;
            }

            return false;
        }

        private async UniTask ShowTimerAsync(CancellationToken cancellationToken)
        {
            _advertisingView.Show();
            
            for (int i = SecondsCount; i > 0 ; i--)
            {
                _advertisingView.TimerText.SetText($"{i}");
                await UniTask.Delay(_timerTimeSpan, cancellationToken: cancellationToken);
            }
            
            _advertisingView.Hide();
        }
    }
}