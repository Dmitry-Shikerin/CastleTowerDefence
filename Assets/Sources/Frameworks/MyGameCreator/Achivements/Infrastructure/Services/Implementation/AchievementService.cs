using System;
using System.Collections.Generic;
using System.Linq;
using Sources.BoundedContexts.Ids.Domain.Constant;
using Sources.BoundedContexts.KillEnemyCounters.Domain.Models.Implementation;
using Sources.Frameworks.MyGameCreator.Achivements.Domain.Configs;
using Sources.Frameworks.MyGameCreator.Achivements.Infrastructure.Services.Interfaces;
using Sources.InfrastructureInterfaces.Services.Repositories;
using UnityEngine.SocialPlatforms.Impl;

namespace Sources.Frameworks.MyGameCreator.Achivements.Infrastructure.Services.Implementation
{
    public class AchievementService : IAchievementService
    {
        private readonly IEntityRepository _entityRepository;
        private readonly Dictionary<string, Achievement> _achievements;
        private readonly Dictionary<string, AchievementConfig> _achievementsConfigs;

        public AchievementService(
            IEntityRepository entityRepository,
            AchievementConfigCollector achievementConfigCollector)
        {
            _entityRepository = entityRepository ?? throw new ArgumentNullException(nameof(entityRepository));
            _achievements = new Dictionary<string, Achievement>();
            _achievementsConfigs = achievementConfigCollector.Configs
                .ToDictionary(config => config.Id, config => config);
        }

        public void Initialize()
        {
        }

        public void Destroy()
        {
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
        
        private void InitializeKillEnemyCounter()
        {
            KillEnemyCounter killEnemyCounter = _entityRepository
                .Get<KillEnemyCounter>(ModelId.KillEnemyCounter);
            
            killEnemyCounter.KillZombiesCountChanged += OnKillZombiesCountChanged;
        }

        private void OnKillZombiesCountChanged()
        {
            
        }
    }
}