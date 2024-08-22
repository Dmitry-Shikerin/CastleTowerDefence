using System;
using Sources.BoundedContexts.Bunkers.Domain;
using Sources.BoundedContexts.GameCompleteds.Infrastructure.Services.Interfaces;
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
        private readonly IGameCompletedService _gameCompletedService;

        private Achievement _achievement;
        private Bunker _bunker;
        private AchievementView _achievementView;

        public CompleteGameWithOneHealthAchievementCommand(
            IEntityRepository entityRepository,
            IPrefabCollector prefabCollector,
            ILoadService loadService,
            IGameCompletedService gameCompletedService,
            AchievementView achievementView,
            DiContainer container) : base(achievementView, prefabCollector, loadService, container)
        {
            _entityRepository = entityRepository ?? 
                                throw new ArgumentNullException(nameof(entityRepository));
            _gameCompletedService = gameCompletedService ??
                                    throw new ArgumentNullException(nameof(gameCompletedService));
        }

        public override void Initialize()
        {
            base.Initialize();
            
            _achievement = _entityRepository.Get<Achievement>(ModelId.CompleteGameWithOneHealthAchievementCommand);
            _bunker = _entityRepository.Get<Bunker>(ModelId.Bunker);
            
            _gameCompletedService.GameCompleted += OnCompleted;
        }

        private void OnCompleted()
        {
            if (_achievement.IsCompleted)
                return;

            if (_bunker.Health != 1)
                return;
            
            Execute(_achievement);
        }

        public override void Destroy()
        {
            _gameCompletedService.GameCompleted -= OnCompleted;
        }
    }
}