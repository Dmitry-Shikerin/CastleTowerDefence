using System;
using Sources.BoundedContexts.Ids.Domain.Constant;
using Sources.BoundedContexts.KillEnemyCounters.Domain.Models.Implementation;
using Sources.Frameworks.MyGameCreator.Achivements.Domain;
using Sources.Frameworks.MyGameCreator.Achivements.Domain.Models;
using Sources.InfrastructureInterfaces.Services.Repositories;

namespace Sources.Frameworks.MyGameCreator.Achivements.Infrastructure.Commands.Implementation
{
    public class FirstKillEnemyAchievementCommand : IAchievementCommand
    {
        private readonly IEntityRepository _entityRepository;
        private KillEnemyCounter _killEnemyCounter;
        private Achievement _achievement;

        public FirstKillEnemyAchievementCommand(
            IEntityRepository entityRepository)
        {
            _entityRepository = entityRepository ?? 
                                throw new ArgumentNullException(nameof(entityRepository));
        }

        public void Initialize()
        {
            _killEnemyCounter = _entityRepository
                .Get<KillEnemyCounter>(ModelId.KillEnemyCounter);
            _achievement = _entityRepository
                .Get<Achievement>(ModelId.FirstEnemyKillAchievement);
            _killEnemyCounter.KillZombiesCountChanged += Execute;
        }

        public void Execute()
        {
            if (_killEnemyCounter.KillZombies <= 0)
                return;
            
            _achievement.IsCompleted = true;
        }

        public void Destroy()
        {
            _killEnemyCounter.KillZombiesCountChanged -= Execute;
        }
    }
}