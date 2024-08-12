using System;
using JetBrains.Annotations;
using Sources.BoundedContexts.Ids.Domain.Constant;
using Sources.BoundedContexts.Scenes.Domain;
using Sources.Frameworks.GameServices.DailyRewards.Domain;
using Sources.Frameworks.GameServices.Loads.Services.Interfaces;
using Sources.Frameworks.GameServices.Volumes.Domain.Models.Implementation;
using Sources.InfrastructureInterfaces.Services.Repositories;
using UnityEngine;

namespace Sources.BoundedContexts.Scenes.Infrastructure.Factories.Domain.Implementation
{
    public class MainMenuModelsLoaderService
    {
        private readonly IEntityRepository _entityRepository;
        private readonly ILoadService _loadService;

        public MainMenuModelsLoaderService(
            IEntityRepository entityRepository,
            ILoadService loadService)
        {
            _entityRepository = entityRepository ?? throw new ArgumentNullException(nameof(entityRepository));
            _loadService = loadService ?? throw new ArgumentNullException(nameof(loadService));
        }

        public MainMenuModel Load()
        {
            _loadService.Load(ModelId.MainMenuModels);
            
            //Volumes
            Volume musicVolume = _entityRepository.Get<Volume>(ModelId.MusicVolume);
            Volume soundsVolume = _entityRepository.Get<Volume>(ModelId.SoundsVolume);
            
            //DailyReward
            DailyReward dailyReward = _entityRepository.Get<DailyReward>(ModelId.DailyReward);
            
            Debug.Log($"Load models");
            
            return new MainMenuModel(
                musicVolume, 
                soundsVolume,
                dailyReward);
        }
    }
}