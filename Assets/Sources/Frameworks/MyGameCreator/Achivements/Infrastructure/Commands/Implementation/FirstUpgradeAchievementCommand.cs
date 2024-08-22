using System;
using Sources.BoundedContexts.Huds.Presentations;
using Sources.BoundedContexts.Ids.Domain.Constant;
using Sources.BoundedContexts.Upgrades.Domain.Models;
using Sources.Frameworks.GameServices.Loads.Services.Interfaces;
using Sources.Frameworks.GameServices.Prefabs.Implementation;
using Sources.Frameworks.MyGameCreator.Achivements.Domain.Models;
using Sources.Frameworks.MyGameCreator.Achivements.Infrastructure.Commands.Implementation.Base;
using Sources.Frameworks.MyGameCreator.Achivements.Presentation;
using Sources.InfrastructureInterfaces.Services.Repositories;
using Zenject;

namespace Sources.Frameworks.MyGameCreator.Achivements.Infrastructure.Commands.Implementation
{
    public class FirstUpgradeAchievementCommand : AchievementCommandBase
    {
        private readonly IEntityRepository _entityRepository;

        private Upgrade _healthUpgrade;
        private Upgrade _attackUpgrade;
        private Upgrade _flamethrowerUpgrade;
        private Upgrade _nukeUpgrade;
        private Achievement _achievement;
        private AchievementView _achievementView;

        public FirstUpgradeAchievementCommand(
            IEntityRepository entityRepository,
            IPrefabCollector prefabCollector,
            ILoadService loadService,
            AchievementView achievementView,
            DiContainer container) 
            : base(
                achievementView, 
                prefabCollector,
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
                .Get<Achievement>(ModelId.FirstUpgradeAchievement);
            _healthUpgrade.LevelChanged += OnCompleted;
            _attackUpgrade.LevelChanged += OnCompleted;
            _flamethrowerUpgrade.LevelChanged += OnCompleted;
            _nukeUpgrade.LevelChanged += OnCompleted;
        }

        private void OnCompleted()
        {
            if (_achievement.IsCompleted)
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