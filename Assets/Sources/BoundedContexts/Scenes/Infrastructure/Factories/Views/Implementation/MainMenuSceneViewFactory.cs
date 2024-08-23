using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Sources.BoundedContexts.Huds.Presentations;
using Sources.BoundedContexts.Ids.Domain.Constant;
using Sources.BoundedContexts.Scenes.Domain;
using Sources.BoundedContexts.Scenes.Infrastructure.Factories.Domain.Implementation;
using Sources.BoundedContexts.Scenes.Infrastructure.Factories.Views.Interfaces;
using Sources.Frameworks.GameServices.DailyRewards.Infrastructure.Factories;
using Sources.Frameworks.GameServices.Loads.Services.Interfaces;
using Sources.Frameworks.GameServices.Prefabs.Implementation;
using Sources.Frameworks.GameServices.Scenes.Domain.Interfaces;
using Sources.Frameworks.GameServices.Volumes.Infrastucture.Factories;
using Sources.Frameworks.MyGameCreator.Achivements.Domain.Configs;
using Sources.Frameworks.MyGameCreator.Achivements.Domain.Models;
using Sources.Frameworks.UiFramework.AudioSources.Infrastructure.Services.AudioService.Interfaces;
using Sources.Frameworks.YandexSdkFramework.Leaderboards.Services.Implementation;
using Sources.InfrastructureInterfaces.Services.Repositories;
using UnityEngine;

namespace Sources.BoundedContexts.Scenes.Infrastructure.Factories.Views.Implementation
{
    public class MainMenuSceneViewFactory : ISceneViewFactory
    {
        private readonly IPrefabCollector _prefabCollector;
        private readonly IEntityRepository _entityRepository;
        private readonly MainMenuHud _mainMenuHud;
        private readonly ILoadService _loadService;
        private readonly MainMenuModelsLoaderService _mainMenuModelsLoaderService;
        private readonly MainMenuModelsCreatorService _mainMenuModelsCreatorService;
        private readonly YandexLeaderboardInitializeService _yandexLeaderboardInitializeService;
        private readonly VolumeViewFactory _volumeViewFactory;
        private readonly DailyRewardViewFactory _dailyRewardViewFactory;

        public MainMenuSceneViewFactory(
            IPrefabCollector prefabCollector,
            IEntityRepository entityRepository,
            MainMenuHud hud,
            ILoadService loadService,
            MainMenuModelsLoaderService mainMenuModelsLoaderService,
            MainMenuModelsCreatorService mainMenuModelsCreatorService,
            YandexLeaderboardInitializeService yandexLeaderboardInitializeService,
            VolumeViewFactory volumeViewFactory,
            DailyRewardViewFactory dailyRewardViewFactory)
        {
            _prefabCollector = prefabCollector ?? throw new ArgumentNullException(nameof(prefabCollector));
            _entityRepository = entityRepository ?? throw new ArgumentNullException(nameof(entityRepository));
            _mainMenuHud = hud ?? throw new ArgumentNullException(nameof(hud));
            _loadService = loadService ?? throw new ArgumentNullException(nameof(loadService));
            _mainMenuModelsLoaderService = mainMenuModelsLoaderService ??
                                           throw new ArgumentNullException(nameof(mainMenuModelsLoaderService));
            _mainMenuModelsCreatorService = mainMenuModelsCreatorService ??
                                            throw new ArgumentNullException(nameof(mainMenuModelsCreatorService));
            _yandexLeaderboardInitializeService = yandexLeaderboardInitializeService ??
                                                  throw new ArgumentNullException(nameof(yandexLeaderboardInitializeService));
            _volumeViewFactory = volumeViewFactory ??
                                 throw new ArgumentNullException(nameof(volumeViewFactory));
            _dailyRewardViewFactory = dailyRewardViewFactory ?? 
                                      throw new ArgumentNullException(nameof(dailyRewardViewFactory));
        }
    
        public void Create(IScenePayload payload)
        {
            MainMenuModel mainMenuModel = Load(payload);
            
            //Volume
            _volumeViewFactory.Create(mainMenuModel.MusicVolume, _mainMenuHud.MusicVolumeView);
            _volumeViewFactory.Create(mainMenuModel.SoundsVolume, _mainMenuHud.SoundVolumeView);
            
            //DailyReward
            _dailyRewardViewFactory.Create(_mainMenuHud.DailyRewardView);
            
            //Achievements
            List<Achievement> achievements = _entityRepository
                .GetAll<Achievement>(ModelId.AchievementModels).ToList();

            if (achievements.Count != _mainMenuHud.AchievementViews.Count)
                throw new IndexOutOfRangeException(nameof(achievements));

            for (int i = 0; i < achievements.Count; i++)
            {
                AchievementConfig config = _prefabCollector
                    .Get<AchievementConfigCollector>()
                    .Configs
                    .First(config => config.Id == achievements[i].Id);
                _mainMenuHud.AchievementViews[i].Construct(achievements[i], config);
            }
            
            //Leaderboard
            _yandexLeaderboardInitializeService.Construct(_mainMenuHud.LeaderBoardElementViews);
        }
        
        private MainMenuModel Load(IScenePayload payload)
        {
            if (_loadService.HasKey(ModelId.SoundsVolume))
            {
                Debug.Log(_loadService.HasKey(ModelId.SoundsVolume));
                Debug.Log($"Load models");
                return _mainMenuModelsLoaderService.Load();
            }
            
            return _mainMenuModelsCreatorService.Load();
        }
    }
}
