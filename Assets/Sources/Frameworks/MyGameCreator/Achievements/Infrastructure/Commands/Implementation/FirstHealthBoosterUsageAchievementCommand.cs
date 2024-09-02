using System;
using Sources.BoundedContexts.HealthBoosters.Domain;
using Sources.Frameworks.GameServices.Loads.Domain.Constant;
using Sources.Frameworks.GameServices.Loads.Services.Interfaces;
using Sources.Frameworks.GameServices.Prefabs.Interfaces;
using Sources.Frameworks.GameServices.Repositories.Services.Interfaces;
using Sources.Frameworks.MyGameCreator.Achievements.Domain.Models;
using Sources.Frameworks.MyGameCreator.Achievements.Infrastructure.Commands.Implementation.Base;
using Sources.Frameworks.MyGameCreator.Achievements.Presentation;
using Zenject;

namespace Sources.Frameworks.MyGameCreator.Achievements.Infrastructure.Commands.Implementation
{
    public class FirstHealthBoosterUsageAchievementCommand : AchievementCommandBase
    {
        private readonly IEntityRepository _entityRepository;
        
        private HealthBooster _healthBooster;
        private Achievement _achievement;
        private AchievementView _achievementView;

        public FirstHealthBoosterUsageAchievementCommand(
            IEntityRepository entityRepository,
            IAssetCollector assetCollector,
            ILoadService loadService,
            AchievementView achievementView,
            DiContainer container) : base(achievementView, assetCollector, loadService, container)
        {
            _entityRepository = entityRepository ?? 
                                throw new ArgumentNullException(nameof(entityRepository));
        }

        public override void Initialize()
        {
            base.Initialize();
            
            _healthBooster = _entityRepository.Get<HealthBooster>(ModelId.HealthBooster);
            _achievement = _entityRepository.Get<Achievement>(ModelId.FirstHealthBoosterUsageAchievement);
            _healthBooster.CountRemoved += OnCompleted;
        }

        private void OnCompleted()
        {
            if (_achievement.IsCompleted)
                return;
            
            Execute(_achievement);
        }

        public override void Destroy()
        {
            _healthBooster.CountRemoved -= OnCompleted;
        }
    }
}