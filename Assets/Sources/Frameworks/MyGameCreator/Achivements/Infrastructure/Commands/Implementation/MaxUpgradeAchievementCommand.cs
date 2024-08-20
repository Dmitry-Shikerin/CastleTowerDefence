using System;
using Sources.BoundedContexts.Huds.Presentations;
using Sources.BoundedContexts.Ids.Domain.Constant;
using Sources.BoundedContexts.Upgrades.Domain.Models;
using Sources.Frameworks.MyGameCreator.Achivements.Domain.Models;
using Sources.Frameworks.MyGameCreator.Achivements.Infrastructure.Commands.Implementation.Base;
using Sources.Frameworks.MyGameCreator.Achivements.Presentation;
using Sources.InfrastructureInterfaces.Services.Repositories;
using Zenject;

namespace Sources.Frameworks.MyGameCreator.Achivements.Infrastructure.Commands.Implementation
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
            AchievementView achievementView,
            DiContainer container) : base(achievementView, container)
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
            
            _achievement.IsCompleted = true;
            
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