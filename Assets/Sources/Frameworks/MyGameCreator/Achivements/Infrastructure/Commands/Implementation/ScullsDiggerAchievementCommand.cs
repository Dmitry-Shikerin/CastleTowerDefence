using System;
using Sources.BoundedContexts.Huds.Presentations;
using Sources.BoundedContexts.Ids.Domain.Constant;
using Sources.BoundedContexts.PlayerWallets.Domain.Models;
using Sources.Frameworks.GameServices.Loads.Services.Interfaces;
using Sources.Frameworks.GameServices.Prefabs.Implementation;
using Sources.Frameworks.GameServices.Prefabs.Interfaces;
using Sources.Frameworks.GameServices.Repositories.Services.Interfaces;
using Sources.Frameworks.MyGameCreator.Achivements.Domain.Models;
using Sources.Frameworks.MyGameCreator.Achivements.Infrastructure.Commands.Implementation.Base;
using Sources.Frameworks.MyGameCreator.Achivements.Presentation;
using Zenject;

namespace Sources.Frameworks.MyGameCreator.Achivements.Infrastructure.Commands.Implementation
{
    public class ScullsDiggerAchievementCommand : AchievementCommandBase
    {
        private readonly IEntityRepository _entityRepository;
        
        private Achievement _achievement;
        private AchievementView _achievementView;
        private PlayerWallet _playerWallet;

        public ScullsDiggerAchievementCommand(
            IEntityRepository entityRepository,
            IAssetCollector assetCollector,
            ILoadService loadService,
            AchievementView achievementView,
            DiContainer container) 
            : base(
                achievementView, 
                assetCollector,
                loadService,
                container)
        {
            _entityRepository = entityRepository ?? 
                                throw new ArgumentNullException(nameof(entityRepository));
        }

        public override void Initialize()
        {
            base.Initialize();

            _playerWallet = _entityRepository.
                Get<PlayerWallet>(ModelId.PlayerWallet);
            _achievement = _entityRepository
                .Get<Achievement>(ModelId.ScullsDiggerAchievement);

            _playerWallet.CoinsChanged += OnCompleted;
        }

        private void OnCompleted()
        {
            if (_achievement.IsCompleted)
                return;
            
            if (_playerWallet.Coins < 100)
                return;
            
            Execute(_achievement);
        }

        public override void Destroy()
        {
            _playerWallet.CoinsChanged -= OnCompleted;
        }
    }
}