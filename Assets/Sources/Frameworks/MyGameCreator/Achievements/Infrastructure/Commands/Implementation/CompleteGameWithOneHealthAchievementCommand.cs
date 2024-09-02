using System;
using Sources.BoundedContexts.Bunkers.Domain;
using Sources.BoundedContexts.GameCompleted.Infrastructure.Services.Interfaces;
using Sources.Frameworks.GameServices.Loads.Domain.Constant;
using Sources.Frameworks.GameServices.Loads.Services.Interfaces;
using Sources.Frameworks.GameServices.Prefabs.Interfaces;
using Sources.Frameworks.GameServices.Repositories.Services.Interfaces;
using Sources.Frameworks.MyGameCreator.Achievements.Domain.Models;
using Sources.Frameworks.MyGameCreator.Achievements.Infrastructure.Commands.Implementation.Base;
using Sources.Frameworks.MyGameCreator.Achievements.Presentation;
using Zenject;

namespace Sources.Frameworks.MyGameCreator.Achievements.Infrastructure.Commands.Implementation
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
            IAssetCollector assetCollector,
            ILoadService loadService,
            IGameCompletedService gameCompletedService,
            AchievementView achievementView,
            DiContainer container) : base(achievementView, assetCollector, loadService, container)
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