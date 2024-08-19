using System;
using Sources.BoundedContexts.Huds.Presentations;
using Sources.BoundedContexts.Ids.Domain.Constant;
using Sources.BoundedContexts.KillEnemyCounters.Domain.Models.Implementation;
using Sources.Frameworks.MyGameCreator.Achivements.Domain.Models;
using Sources.Frameworks.MyGameCreator.Achivements.Infrastructure.Commands.Implementation.Base;
using Sources.Frameworks.MyGameCreator.Achivements.Presentation;
using Sources.InfrastructureInterfaces.Services.Repositories;
using Zenject;

namespace Sources.Frameworks.MyGameCreator.Achivements.Infrastructure.Commands.Implementation
{
    public class FirstKillEnemyAchievementCommand : AchievementCommandBase
    {
        private readonly IEntityRepository _entityRepository;
        
        private KillEnemyCounter _killEnemyCounter;
        private Achievement _achievement;
        private AchievementView _achievementView;

        public FirstKillEnemyAchievementCommand(
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
            
            _killEnemyCounter = _entityRepository
                .Get<KillEnemyCounter>(ModelId.KillEnemyCounter);
            _achievement = _entityRepository
                .Get<Achievement>(ModelId.FirstEnemyKillAchievement);
            _killEnemyCounter.KillZombiesCountChanged += Execute;
        }

        public override void Execute()
        {
            if (_achievement.IsCompleted)
                return;
            
            if (_killEnemyCounter.KillZombies <= 0)
                return;
            
            _achievement.IsCompleted = true;
            _achievementView.Construct(_achievement);
            
            base.Execute();
        }

        public override void Destroy()
        {
            _killEnemyCounter.KillZombiesCountChanged -= Execute;
        }
    }
}