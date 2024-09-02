using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.Utilities;
using Sources.Frameworks.GameServices.Loads.Domain.Constant;
using Sources.Frameworks.GameServices.Prefabs.Interfaces;
using Sources.Frameworks.GameServices.Repositories.Services.Interfaces;
using Sources.Frameworks.MyGameCreator.Achievements.Domain.Configs;
using Sources.Frameworks.MyGameCreator.Achievements.Domain.Models;
using Sources.Frameworks.MyGameCreator.Achievements.Infrastructure.Commands.Implementation;
using Sources.Frameworks.MyGameCreator.Achievements.Infrastructure.Commands.Interfaces;
using Sources.Frameworks.MyGameCreator.Achievements.Infrastructure.Services.Interfaces;

namespace Sources.Frameworks.MyGameCreator.Achievements.Infrastructure.Services.Implementation
{
    public class AchievementService : IAchievementService
    {
        private readonly IEntityRepository _entityRepository;
        private readonly IAssetCollector _assetCollector;
        private readonly Dictionary<string, Achievement> _achievements;
        private Dictionary<string, AchievementConfig> _achievementsConfigs;
        private readonly IEnumerable<IAchievementCommand> _achievementCommands;
        
        public AchievementService(
            IEntityRepository entityRepository,
            IAssetCollector assetCollector,
            FirstKillEnemyAchievementCommand firstKillEnemyAchievementCommand,
            FirstUpgradeAchievementCommand firstUpgradeAchievementCommand,
            FirstHealthBoosterUsageAchievementCommand firstHealthBoosterUsageAchievementCommand,
            FirstWaveCompletedAchievementCommand firstWaveCompletedAchievementCommand,
            ScullsDiggerAchievementCommand scullsDiggerAchievementCommand,
            MaxUpgradeAchievementCommand maxUpgradeAchievementCommand,
            FiftyWaveCompletedAchievementCommand fiftyWaveCompletedAchievementCommand,
            AllAbilitiesUsedAchievementCommand allAbilitiesUsedAchievementCommand,
            CompleteGameWithOneHealthAchievementCommand completeGameWithOneHealthAchievementCommand)
        {
            _entityRepository = entityRepository ?? 
                                throw new ArgumentNullException(nameof(entityRepository));
            _assetCollector = assetCollector ??
                               throw new ArgumentNullException(nameof(assetCollector));
            _achievements = new Dictionary<string, Achievement>();
            _achievementCommands = new List<IAchievementCommand>()
            {
                firstKillEnemyAchievementCommand,
                firstUpgradeAchievementCommand,
                firstHealthBoosterUsageAchievementCommand,
                firstWaveCompletedAchievementCommand,
                scullsDiggerAchievementCommand,
                maxUpgradeAchievementCommand,
                fiftyWaveCompletedAchievementCommand,
                allAbilitiesUsedAchievementCommand,
                completeGameWithOneHealthAchievementCommand,
            };
        }

        public void Initialize()
        {
            _entityRepository
                .GetAll<Achievement>(ModelId.GetIds<Achievement>())
                .ToDictionary(achievement => achievement.Id, achievement => achievement);
            _achievementsConfigs = _assetCollector
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