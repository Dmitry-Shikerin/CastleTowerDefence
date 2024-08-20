﻿using System;
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
            AchievementView achievementView,
            DiContainer container) : base(achievementView, container)
        {
            _entityRepository = entityRepository ?? 
                                throw new ArgumentNullException(nameof(entityRepository));
        }

        public override void Initialize()
        {
            base.Initialize();
            
            _healthBooster = _entityRepository.Get<HealthBooster>(ModelId.HealthBooster);
            _achievement = _entityRepository.Get<Achievement>(ModelId.FirstHealthBoosterUsageAchievement);
            _healthBooster.CountRemoved += OnCompleted;
        }

        private void OnCompleted()
        {
            if (_achievement.IsCompleted)
                return;
            
            _achievement.IsCompleted = true;
            
            Execute(_achievement);
        }

        public override void Destroy()
        {
            _healthBooster.CountRemoved -= OnCompleted;
        }
    }
}