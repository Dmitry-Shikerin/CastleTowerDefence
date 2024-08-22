﻿using System;
using Sources.BoundedContexts.EnemySpawners.Domain.Models;
using Sources.BoundedContexts.Ids.Domain.Constant;
using Sources.Frameworks.MVPPassiveView.Controllers.Interfaces.ControllerLifetimes;
using Sources.Frameworks.YandexSdcFramework.ServicesInterfaces.AdverticingServices;
using Sources.InfrastructureInterfaces.Services.Repositories;

namespace Sources.BoundedContexts.AdvertisingAfterWaves.Infrrastructure.Services
{
    public class AdvertisingAfterWaveService : IInitialize, IDestroy
    {
        private const int WavesCount = 20;
        
        private readonly IEntityRepository _entityRepository;
        private readonly IInterstitialAdService _interstitialAdService;
        private EnemySpawner _enemySpawner;

        public AdvertisingAfterWaveService(
            IEntityRepository entityRepository, 
            IInterstitialAdService interstitialAdService)
        {
            _entityRepository = entityRepository ?? throw new ArgumentNullException(nameof(entityRepository));
            _interstitialAdService = interstitialAdService ?? 
                                  throw new ArgumentNullException(nameof(interstitialAdService));
        }

        public void Initialize()
        {
            _enemySpawner = _entityRepository.Get<EnemySpawner>(ModelId.EnemySpawner);
            _enemySpawner.WaveChanged += OnShowInterstitial;
        }

        public void Destroy()
        {
            _enemySpawner.WaveChanged -= OnShowInterstitial;   
        }

        private void OnShowInterstitial()
        {
            if (CheckShow() == false)
                return;
            
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
    }
}