using System;
using Sources.BoundedContexts.HealthBoosters.Domain;
using Sources.BoundedContexts.Ids.Domain.Constant;
using Sources.Frameworks.GameServices.Loads.Services.Interfaces;
using Sources.Frameworks.GameServices.Prefabs.Implementation;
using Sources.Frameworks.MyGameCreator.Achivements.Domain.Models;
using Sources.Frameworks.MyGameCreator.Achivements.Infrastructure.Commands.Implementation.Base;
using Sources.Frameworks.MyGameCreator.Achivements.Presentation;
using Sources.InfrastructureInterfaces.Services.Repositories;
using Zenject;

namespace Sources.Frameworks.MyGameCreator.Achivements.Infrastructure.Commands.Implementation
{
    public class FirstHealthBoosterUsageAchievementCommand : AchievementCommandBase
    {
        private readonly IEntityRepository _entityRepository;
        
        private HealthBooster _healthBooster;
        private Achievement _achievement;
        private AchievementView _achievementView;

        public FirstHealthBoosterUsageAchievementCommand(
            IEntityRepository entityRepository,
            IPrefabCollector prefabCollector,
            ILoadService loadService,
            AchievementView achievementView,
            DiContainer container) : base(achievementView, prefabCollector, loadService, container)
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
            
            _achievement.IsCompleted = true;
            
            Execute(_achievement);
        }

        public override void Destroy()
        {
            _healthBooster.CountRemoved -= OnCompleted;
        }
    }
}