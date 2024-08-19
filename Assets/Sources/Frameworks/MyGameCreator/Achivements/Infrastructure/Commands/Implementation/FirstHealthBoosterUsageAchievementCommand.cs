using System;
using Doozy.Runtime.Signals;
using JetBrains.Annotations;
using Sources.BoundedContexts.HealthBoosters.Domain;
using Sources.BoundedContexts.Huds.Presentations;
using Sources.BoundedContexts.Ids.Domain.Constant;
using Sources.Frameworks.DoozyWrappers.SignalBuses.Domain.Constants;
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
            
            _healthBooster = _entityRepository.Get<HealthBooster>(ModelId.HealthBooster);
            _achievement = _entityRepository.Get<Achievement>(ModelId.FirstHealthBoosterUsageAchievement);
            _healthBooster.CountRemoved += Execute;
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
            _healthBooster.CountRemoved -= Execute;
        }
    }
}