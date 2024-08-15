using System;
using Doozy.Runtime.Signals;
using JetBrains.Annotations;
using Sources.BoundedContexts.HealthBoosters.Domain;
using Sources.BoundedContexts.Huds.Presentations;
using Sources.BoundedContexts.Ids.Domain.Constant;
using Sources.Frameworks.DoozyWrappers.SignalBuses.Domain.Constants;
using Sources.Frameworks.MyGameCreator.Achivements.Domain.Models;
using Sources.Frameworks.MyGameCreator.Achivements.Presentation;
using Sources.InfrastructureInterfaces.Services.Repositories;
using Zenject;

namespace Sources.Frameworks.MyGameCreator.Achivements.Infrastructure.Commands.Implementation
{
    public class FirstHealthBoosterUsageAchievementCommand : IAchievementCommand
    {
        private readonly IEntityRepository _entityRepository;
        private readonly DiContainer _container;
        
        private HealthBooster _healthBooster;
        private Achievement _achievement;
        private AchievementView _achievementView;
        private SignalStream _stream;

        public FirstHealthBoosterUsageAchievementCommand(
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
            _healthBooster = _entityRepository.Get<HealthBooster>(ModelId.HealthBooster);
            _achievement = _entityRepository.Get<Achievement>(ModelId.FirstHealthBoosterUsageAchievement);
            _stream = SignalStream.Get(StreamConst.Gameplay, StreamConst.ReceivedAchievement);
            _healthBooster.CountRemoved += Execute;
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
            _healthBooster.CountRemoved -= Execute;
        }
    }
}