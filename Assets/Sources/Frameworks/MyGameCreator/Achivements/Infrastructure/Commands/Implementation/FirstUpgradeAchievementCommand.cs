using System;
using Doozy.Runtime.Signals;
using Sources.BoundedContexts.Huds.Presentations;
using Sources.BoundedContexts.Ids.Domain.Constant;
using Sources.BoundedContexts.Upgrades.Domain.Models;
using Sources.Frameworks.DoozyWrappers.SignalBuses.Domain.Constants;
using Sources.Frameworks.MyGameCreator.Achivements.Domain.Models;
using Sources.Frameworks.MyGameCreator.Achivements.Presentation;
using Sources.InfrastructureInterfaces.Services.Repositories;
using UnityEngine;
using Zenject;

namespace Sources.Frameworks.MyGameCreator.Achivements.Infrastructure.Commands.Implementation
{
    public class FirstUpgradeAchievementCommand : IAchievementCommand
    {
        private readonly IEntityRepository _entityRepository;
        private readonly DiContainer _container;
        private Upgrade _healthUpgrade;
        private Upgrade _attackUpgrade;
        private Upgrade _flamethrowerUpgrade;
        private Upgrade _nukeUpgrade;
        private Achievement _achievement;
        private AchievementView _achievementView;
        private SignalStream _stream;

        public FirstUpgradeAchievementCommand(
            IEntityRepository entityRepository,
            GameplayHud hud,
            DiContainer container)
        {
            if (hud == null)
                throw new ArgumentNullException(nameof(hud));
            
            _achievementView = hud.PopUpAchievementView ?? 
                               throw new ArgumentNullException(nameof(_achievementView));
            _entityRepository = entityRepository ?? 
                                throw new ArgumentNullException(nameof(entityRepository));
            _container = container ?? throw new ArgumentNullException(nameof(container));
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
            
            _stream = SignalStream.Get(StreamConst.Gameplay, StreamConst.ReceivedAchievement);
        }

        public void Execute()
        {
            _achievement.IsCompleted = true;
            
            _container.Inject(_achievementView);
            _achievementView.Construct(_achievement);
            _stream.SendSignal(true);
            
            Destroy();
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