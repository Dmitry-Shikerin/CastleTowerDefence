using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Sources.BoundedContexts.Ids.Domain.Constant;
using Sources.BoundedContexts.Scenes.Domain;
using Sources.Frameworks.GameServices.DailyRewards.Domain;
using Sources.Frameworks.GameServices.Loads.Services.Interfaces;
using Sources.Frameworks.GameServices.Repositories.Services.Interfaces;
using Sources.Frameworks.GameServices.Volumes.Domain.Models.Implementation;
using Sources.Frameworks.MyGameCreator.Achivements.Domain.Models;
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
            
            foreach (string id in ModelId.AchievementModels)
            {
                Achievement achievement = new Achievement(id);
                _entityRepository.Add(achievement);
                achievements.Add(achievement);
            }
            
            //Volume
            Volume musicVolume = new Volume(ModelId.MusicVolume);
            _entityRepository.Add(musicVolume);
            Volume soundsVolume = new Volume(ModelId.SoundsVolume);
            _entityRepository.Add(soundsVolume);
            
            //DailyReward
            DailyReward dailyReward = new DailyReward(ModelId.DailyReward);
            _entityRepository.Add(dailyReward);
            
            _loadService.Save(ModelId.MainMenuModels);
            _loadService.Save(ModelId.AchievementModels);
            Debug.Log($"Create models");
            
            return new MainMenuModel(
                musicVolume, 
                soundsVolume,
                dailyReward,
                achievements);
        }
    }
}