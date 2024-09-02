using System;
using System.Collections.Generic;
using Sources.BoundedContexts.HealthBoosters.Domain;
using Sources.BoundedContexts.Scenes.Domain;
using Sources.Frameworks.GameServices.DailyRewards.Domain;
using Sources.Frameworks.GameServices.Loads.Domain.Constant;
using Sources.Frameworks.GameServices.Loads.Services.Interfaces;
using Sources.Frameworks.GameServices.Repositories.Services.Interfaces;
using Sources.Frameworks.GameServices.Volumes.Domain.Models.Implementation;
using Sources.Frameworks.MyGameCreator.Achievements.Domain.Models;
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
            //Achievements
            List<Achievement> achievements = new List<Achievement>();
            
            foreach (string id in ModelId.GetIds<Achievement>())
            {
                Achievement achievement = _loadService.Load<Achievement>(id);
                achievements.Add(achievement);
            }
            
            //Volumes
            Volume musicVolume = _loadService.Load<Volume>(ModelId.MusicVolume);
            Volume soundsVolume = _loadService.Load<Volume>(ModelId.SoundsVolume);
            
            //DailyReward
            DailyReward dailyReward = _loadService.Load<DailyReward>(ModelId.DailyReward);
            
            //HealthBooster
            HealthBooster healthBooster = _loadService.Load<HealthBooster>(ModelId.HealthBooster);
            
            Debug.Log($"Load models");
            
            return new MainMenuModel(
                musicVolume, 
                soundsVolume,
                dailyReward,
                achievements,
                healthBooster);
        }
    }
}