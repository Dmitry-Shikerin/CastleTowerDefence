using System;
using Sources.BoundedContexts.HealthBoosters.Domain;
using Sources.BoundedContexts.Ids.Domain.Constant;
using Sources.Frameworks.GameServices.Loads.Services.Interfaces;
using Sources.Frameworks.GameServices.Prefabs.Implementation;
using Sources.Frameworks.MyGameCreator.Achivements.Domain.Models;
using Sources.Frameworks.MyGameCreator.Achivements.Infrastructure.Commands.Implementation.Base;
using Sources.Frameworks.MyGameCreator.Achivements.Presentation;
using Sources.InfrastructureInterfaces.Services.Repositories;
using Zenject;

namespace Sources.Frameworks.MyGameCreator.Achivements.Infrastructure.Commands.Implementation
{
    public class CompleteGameWithOneHealthAchievementCommand : AchievementCommandBase
    {
        private readonly IEntityRepository _entityRepository;
        
        private Achievement _achievement;
        private AchievementView _achievementView;

        public CompleteGameWithOneHealthAchievementCommand(
            IEntityRepository entityRepository,
            IPrefabCollector prefabCollector,
            ILoadService loadService,
            AchievementView achievementView,
            DiContainer container) : base(achievementView, prefabCollector, loadService, container)
        {
            _entityRepository = entityRepository ?? 
                                throw new ArgumentNullException(nameof(entityRepository));
        }

        public override void Initialize()
        {
            base.Initialize();
            
            _achievement = _entityRepository.Get<Achievement>(ModelId.CompleteGameWithOneHealthAchievementCommand);
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
            
        }
    }
}