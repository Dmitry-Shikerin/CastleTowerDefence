using System;
using JetBrains.Annotations;
using Sources.BoundedContexts.HealthBoosters.Domain;
using Sources.BoundedContexts.Ids.Domain.Constant;
using Sources.Frameworks.MyGameCreator.Achivements.Domain.Models;
using Sources.InfrastructureInterfaces.Services.Repositories;

namespace Sources.Frameworks.MyGameCreator.Achivements.Infrastructure.Commands.Implementation
{
    public class FirstHealthBoosterUsageAchievementCommand : IAchievementCommand
    {
        private readonly IEntityRepository _entityRepository;
        
        private HealthBooster _healthBooster;
        private Achievement _achievement;

        public FirstHealthBoosterUsageAchievementCommand(IEntityRepository entityRepository)
        {
            _entityRepository = entityRepository ?? throw new ArgumentNullException(nameof(entityRepository));
        }

        public void Initialize()
        {
            _healthBooster = _entityRepository.Get<HealthBooster>(ModelId.HealthBooster);
            _achievement = _entityRepository.Get<Achievement>(ModelId.FirstHealthBoosterUsageAchievement);
            _healthBooster.CountRemoved += Execute;
        }

        public void Execute()
        {
            _achievement.IsCompleted = true;
        }

        public void Destroy()
        {
            _healthBooster.CountRemoved -= Execute;
        }
    }
}