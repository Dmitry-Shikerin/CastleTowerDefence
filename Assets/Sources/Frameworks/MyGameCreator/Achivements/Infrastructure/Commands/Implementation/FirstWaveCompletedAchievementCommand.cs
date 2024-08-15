using System;
using Sources.BoundedContexts.EnemySpawners.Domain.Models;
using Sources.BoundedContexts.Huds.Presentations;
using Sources.BoundedContexts.Ids.Domain.Constant;
using Sources.Frameworks.MyGameCreator.Achivements.Domain.Models;
using Sources.Frameworks.MyGameCreator.Achivements.Infrastructure.Commands.Implementation.Base;
using Sources.Frameworks.MyGameCreator.Achivements.Presentation;
using Sources.InfrastructureInterfaces.Services.Repositories;
using Zenject;

namespace Sources.Frameworks.MyGameCreator.Achivements.Infrastructure.Commands.Implementation
{
    public class FirstWaveCompletedAchievementCommand : AchievementCommandBase
    {
        private readonly IEntityRepository _entityRepository;
        
        private Achievement _achievement;
        private AchievementView _achievementView;
        private EnemySpawner _enemySpawner;

        public FirstWaveCompletedAchievementCommand(
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

            _enemySpawner = _entityRepository.
                Get<EnemySpawner>(ModelId.EnemySpawner);
            _achievement = _entityRepository
                .Get<Achievement>(ModelId.FirstWaveCompletedAchievement);

            _enemySpawner.WaveChanged += Execute;
        }

        public override void Execute()
        {
            _achievement.IsCompleted = true;
            _achievementView.Construct(_achievement);
            
            base.Execute();
        }

        public override void Destroy() => 
            _enemySpawner.WaveChanged -= Execute;
    }
}