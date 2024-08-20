﻿using System;
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
            AchievementView achievementView,
            DiContainer container) : base(achievementView, container)
        {
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

            _enemySpawner.WaveChanged += OnCompleted;
        }

        private void OnCompleted()
        {
            if (_achievement.IsCompleted)
                return;
            
            _achievement.IsCompleted = true;
            
           Execute(_achievement);
        }

        public override void Destroy() => 
            _enemySpawner.WaveChanged -= OnCompleted;
    }
}