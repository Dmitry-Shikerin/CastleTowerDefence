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
            GameplayHud hud,
            DiContainer container) : base(hud, container)
        {
            if (hud == null)
                throw new ArgumentNullException(nameof(hud));
            
            _achievementView = hud.PopUpAchievementView ?? 
                               throw new ArgumentNullException(nameof(_achievementView));
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
            _healthUpgrade.LevelChanged += Execute;
            _attackUpgrade.LevelChanged += Execute;
            _flamethrowerUpgrade.LevelChanged += Execute;
            _nukeUpgrade.LevelChanged += Execute;
        }

        public override void Execute()
        {
            if (_achievement.IsCompleted)
                return;
            
            _achievement.IsCompleted = true;
            _achievementView.Construct(_achievement);
            
            base.Execute();
        }

        public override void Destroy()
        {
            _healthUpgrade.LevelChanged -= Execute;
            _attackUpgrade.LevelChanged -= Execute;
            _flamethrowerUpgrade.LevelChanged -= Execute;
            _nukeUpgrade.LevelChanged -= Execute;
        }
    }
}