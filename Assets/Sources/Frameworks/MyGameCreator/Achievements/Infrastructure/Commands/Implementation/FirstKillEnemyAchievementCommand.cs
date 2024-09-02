using System;
using Sources.BoundedContexts.KillEnemyCounters.Domain.Models.Implementation;
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
    public class FirstKillEnemyAchievementCommand : AchievementCommandBase
    {
        private readonly IEntityRepository _entityRepository;
        
        private KillEnemyCounter _killEnemyCounter;
        private Achievement _achievement;
        private AchievementView _achievementView;

        public FirstKillEnemyAchievementCommand(
            IEntityRepository entityRepository,
            IAssetCollector assetCollector,
            ILoadService loadService,
            AchievementView achievementView,
            DiContainer container) 
            : base(
                achievementView, 
                assetCollector,
                loadService,
                container)
        {
            _entityRepository = entityRepository ?? 
                                throw new ArgumentNullException(nameof(entityRepository));
        }

        public override void Initialize()
        {
            base.Initialize();
            
            _killEnemyCounter = _entityRepository
                .Get<KillEnemyCounter>(ModelId.KillEnemyCounter);
            _achievement = _entityRepository
                .Get<Achievement>(ModelId.FirstEnemyKillAchievement);
            _killEnemyCounter.KillZombiesCountChanged += OnCompleted;
        }

        private void OnCompleted()
        {
            if (_achievement.IsCompleted)
                return;
            
            if (_killEnemyCounter.KillZombies <= 0)
                return;
            
            Execute(_achievement);
        }

        public override void Destroy()
        {
            _killEnemyCounter.KillZombiesCountChanged -= OnCompleted;
        }
    }
}