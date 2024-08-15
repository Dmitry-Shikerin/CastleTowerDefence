using System;
using Doozy.Runtime.Signals;
using Sources.BoundedContexts.Huds.Presentations;
using Sources.BoundedContexts.Ids.Domain.Constant;
using Sources.BoundedContexts.KillEnemyCounters.Domain.Models.Implementation;
using Sources.Frameworks.DoozyWrappers.SignalBuses.Domain.Constants;
using Sources.Frameworks.MyGameCreator.Achivements.Domain.Models;
using Sources.Frameworks.MyGameCreator.Achivements.Presentation;
using Sources.InfrastructureInterfaces.Services.Repositories;
using Zenject;

namespace Sources.Frameworks.MyGameCreator.Achivements.Infrastructure.Commands.Implementation
{
    public class FirstKillEnemyAchievementCommand : IAchievementCommand
    {
        private readonly IEntityRepository _entityRepository;
        private readonly DiContainer _container;
        
        private KillEnemyCounter _killEnemyCounter;
        private Achievement _achievement;
        private SignalStream _stream;
        private AchievementView _achievementView;

        public FirstKillEnemyAchievementCommand(
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
            _killEnemyCounter = _entityRepository
                .Get<KillEnemyCounter>(ModelId.KillEnemyCounter);
            _achievement = _entityRepository
                .Get<Achievement>(ModelId.FirstEnemyKillAchievement);
            _killEnemyCounter.KillZombiesCountChanged += Execute;
            
            _stream = SignalStream.Get(StreamConst.Gameplay, StreamConst.ReceivedAchievement);
        }

        public void Execute()
        {
            if (_killEnemyCounter.KillZombies <= 0)
                return;
            
            _achievement.IsCompleted = true;
            _container.Inject(_achievementView);
            _achievementView.Construct(_achievement);
            _stream.SendSignal(true);
            
            Destroy();
        }

        public void Destroy()
        {
            _killEnemyCounter.KillZombiesCountChanged -= Execute;
        }
    }
}