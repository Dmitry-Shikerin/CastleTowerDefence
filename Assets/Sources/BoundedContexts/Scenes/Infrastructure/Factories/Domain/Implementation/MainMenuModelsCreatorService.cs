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
    public class MainMenuModelsCreatorService
    {
        private readonly ILoadService _loadService;
        private readonly IEntityRepository _entityRepository;

        public MainMenuModelsCreatorService(
            ILoadService loadService,
            IEntityRepository entityRepository)
        {
            _loadService = loadService ?? throw new ArgumentNullException(nameof(loadService));
            _entityRepository = entityRepository ?? throw new ArgumentNullException(nameof(entityRepository));
        }

        public MainMenuModel Load()
        {
            //Achievements
            List<Achievement> achievements = new List<Achievement>();
            
            foreach (string id in ModelId.GetIds<Achievement>())
            {
                Achievement achievement = new Achievement(id);
                _entityRepository.Add(achievement);
                achievements.Add(achievement);
            }
            
            //Volume
            Volume musicVolume = new Volume()
            {
                Id = ModelId.MusicVolume,
            };
            _entityRepository.Add(musicVolume);
            Volume soundsVolume = new Volume()
            {
                Id = ModelId.SoundsVolume,
            };
            _entityRepository.Add(soundsVolume);
            
            //DailyReward
            DailyReward dailyReward = new DailyReward()
            {
                Id = ModelId.DailyReward,
            };
            _entityRepository.Add(dailyReward);
            
            //HealthBooster
            HealthBooster healthBooster = new HealthBooster()
            {
                Id = ModelId.HealthBooster,
            };
            _entityRepository.Add(healthBooster);
            
            _loadService.SaveAll();
            Debug.Log($"Create models");
            
            return new MainMenuModel(
                musicVolume, 
                soundsVolume,
                dailyReward,
                achievements,
                healthBooster);
        }
    }
}