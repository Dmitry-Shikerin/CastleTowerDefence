using System;
using Sources.BoundedContexts.Upgrades.Domain.Models;
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
    public class MaxUpgradeAchievementCommand : AchievementCommandBase
    {
        private readonly IEntityRepository _entityRepository;

        private Upgrade _healthUpgrade;
        private Upgrade _attackUpgrade;
        private Upgrade _flamethrowerUpgrade;
        private Upgrade _nukeUpgrade;
        private Achievement _achievement;
        private AchievementView _achievementView;

        public MaxUpgradeAchievementCommand(
            IEntityRepository entityRepository,
            IAssetCollector assetCollector,
            AchievementView achievementView,
            ILoadService loadService,
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
            
            _healthUpgrade = _entityRepository
                .Get<Upgrade>(ModelId.HealthUpgrade);
            _attackUpgrade = _entityRepository
                .Get<Upgrade>(ModelId.AttackUpgrade);
            _flamethrowerUpgrade = _entityRepository
                .Get<Upgrade>(ModelId.FlamethrowerUpgrade);
            _nukeUpgrade = _entityRepository
                .Get<Upgrade>(ModelId.NukeUpgrade);
            _achievement = _entityRepository
                .Get<Achievement>(ModelId.MaxUpgradeAchievement);
            
            _healthUpgrade.LevelChanged += OnCompleted;
            _attackUpgrade.LevelChanged += OnCompleted;
            _flamethrowerUpgrade.LevelChanged += OnCompleted;
            _nukeUpgrade.LevelChanged += OnCompleted;
        }

        private void OnCompleted()
        {
            if (_achievement.IsCompleted)
                return;
            
            if (_healthUpgrade.CurrentLevel != _healthUpgrade.MaxLevel)
                return;
            
            if (_attackUpgrade.CurrentLevel != _attackUpgrade.MaxLevel)
                return;
            
            if (_flamethrowerUpgrade.CurrentLevel != _flamethrowerUpgrade.MaxLevel)
                return;
            
            if (_nukeUpgrade.CurrentLevel != _nukeUpgrade.MaxLevel)
                return;
            
            Execute(_achievement);
        }

        public override void Destroy()
        {
            _healthUpgrade.LevelChanged -= OnCompleted;
            _attackUpgrade.LevelChanged -= OnCompleted;
            _flamethrowerUpgrade.LevelChanged -= OnCompleted;
            _nukeUpgrade.LevelChanged -= OnCompleted;
        }
    }
}