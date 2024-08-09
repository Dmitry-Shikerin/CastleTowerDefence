using System;
using Sources.BoundedContexts.Ids.Domain.Constant;
using Sources.BoundedContexts.Upgrades.Domain.Models;
using Sources.Frameworks.MyGameCreator.Achivements.Domain.Models;
using Sources.InfrastructureInterfaces.Services.Repositories;

namespace Sources.Frameworks.MyGameCreator.Achivements.Infrastructure.Commands.Implementation
{
    public class FirstUpgradeAchievementCommand : IAchievementCommand
    {
        private readonly IEntityRepository _entityRepository;
        private Upgrade _healthUpgrade;
        private Upgrade _attackUpgrade;
        private Upgrade _flamethrowerUpgrade;
        private Upgrade _nukeUpgrade;
        private Achievement _achievement;

        public FirstUpgradeAchievementCommand(
            IEntityRepository entityRepository)
        {
            _entityRepository = entityRepository ?? throw new ArgumentNullException(nameof(entityRepository));
        }

        public void Initialize()
        {
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
            _healthUpgrade.LevelChanged += Execute;
            _attackUpgrade.LevelChanged += Execute;
            _flamethrowerUpgrade.LevelChanged += Execute;
            _nukeUpgrade.LevelChanged += Execute;
        }

        public void Execute()
        {
            _achievement.IsCompleted = true;
        }

        public void Destroy()
        {
            _healthUpgrade.LevelChanged -= Execute;
            _attackUpgrade.LevelChanged -= Execute;
            _flamethrowerUpgrade.LevelChanged -= Execute;
            _nukeUpgrade.LevelChanged -= Execute;
        }
    }
}