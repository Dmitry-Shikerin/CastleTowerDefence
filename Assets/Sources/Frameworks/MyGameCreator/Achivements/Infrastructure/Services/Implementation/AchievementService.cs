using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Sirenix.Utilities;
using Sources.BoundedContexts.Ids.Domain.Constant;
using Sources.Frameworks.GameServices.Prefabs.Implementation;
using Sources.Frameworks.MyGameCreator.Achivements.Domain;
using Sources.Frameworks.MyGameCreator.Achivements.Domain.Configs;
using Sources.Frameworks.MyGameCreator.Achivements.Domain.Models;
using Sources.Frameworks.MyGameCreator.Achivements.Infrastructure.Commands;
using Sources.Frameworks.MyGameCreator.Achivements.Infrastructure.Commands.Implementation;
using Sources.Frameworks.MyGameCreator.Achivements.Infrastructure.Services.Interfaces;
using Sources.InfrastructureInterfaces.Services.Repositories;

namespace Sources.Frameworks.MyGameCreator.Achivements.Infrastructure.Services.Implementation
{
    public class AchievementService : IAchievementService
    {
        private readonly IEntityRepository _entityRepository;
        private readonly IPrefabCollector _prefabCollector;
        private readonly Dictionary<string, Achievement> _achievements;
        private Dictionary<string, AchievementConfig> _achievementsConfigs;
        private readonly IEnumerable<IAchievementCommand> _achievementCommands;
        
        public AchievementService(
            IEntityRepository entityRepository,
            IPrefabCollector prefabCollector,
            FirstKillEnemyAchievementCommand firstKillEnemyAchievementCommand,
            FirstUpgradeAchievementCommand firstUpgradeAchievementCommand,
            FirstHealthBoosterUsageAchievementCommand firstHealthBoosterUsageAchievementCommand,
            FirstWaveCompletedAchievementCommand firstWaveCompletedAchievementCommand,
            ScullsDiggerAchievementCommand scullsDiggerAchievementCommand,
            MaxUpgradeAchievementCommand maxUpgradeAchievementCommand)
        {
            _entityRepository = entityRepository ?? 
                                throw new ArgumentNullException(nameof(entityRepository));
            _prefabCollector = prefabCollector ??
                               throw new ArgumentNullException(nameof(prefabCollector));
            _achievements = new Dictionary<string, Achievement>();
            _achievementCommands = new List<IAchievementCommand>()
            {
                firstKillEnemyAchievementCommand,
                firstUpgradeAchievementCommand,
                firstHealthBoosterUsageAchievementCommand,
                firstWaveCompletedAchievementCommand,
                scullsDiggerAchievementCommand,
                maxUpgradeAchievementCommand,
            };
        }

        public void Initialize()
        {
            _entityRepository
                .GetAll<Achievement>(ModelId.AchievementModels)
                .ToDictionary(achievement => achievement.Id, achievement => achievement);
            _achievementsConfigs = _prefabCollector
                .Get<AchievementConfigCollector>()
                .Configs
                .ToDictionary(config => config.Id, config => config);
            
            _achievementCommands.ForEach(command => command.Initialize());
        }

        public void Destroy()
        {
            _achievementCommands.ForEach(command => command.Destroy());
        }

        public AchievementConfig GetConfig(string id)
        {
            if (_achievementsConfigs.ContainsKey(id) == false)
                throw new KeyNotFoundException(id);
            
            return _achievementsConfigs[id];
        }

        public void Register()
        {
        }
    }
}