using System;
using Sources.BoundedContexts.EnemySpawners.Domain.Models;
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
    public class FiftyWaveCompletedAchievementCommand : AchievementCommandBase
    {
        private readonly IEntityRepository _entityRepository;
        
        private Achievement _achievement;
        private AchievementView _achievementView;
        private EnemySpawner _enemySpawner;

        public FiftyWaveCompletedAchievementCommand(
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

            _enemySpawner = _entityRepository.
                Get<EnemySpawner>(ModelId.EnemySpawner);
            _achievement = _entityRepository
                .Get<Achievement>(ModelId.FiftyWaveCompletedAchievement);

            _enemySpawner.WaveChanged += OnCompleted;
        }

        private void OnCompleted()
        {
            if (_achievement.IsCompleted)
                return;

            if (_enemySpawner.CurrentWaveNumber != 50)
                return;
            
            Execute(_achievement);
        }

        public override void Destroy() => 
            _enemySpawner.WaveChanged -= OnCompleted;
    }
}