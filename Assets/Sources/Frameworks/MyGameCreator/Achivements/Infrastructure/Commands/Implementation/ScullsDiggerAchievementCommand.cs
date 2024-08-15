using System;
using Sources.BoundedContexts.Huds.Presentations;
using Sources.BoundedContexts.Ids.Domain.Constant;
using Sources.BoundedContexts.PlayerWallets.Domain.Models;
using Sources.Frameworks.MyGameCreator.Achivements.Domain.Models;
using Sources.Frameworks.MyGameCreator.Achivements.Infrastructure.Commands.Implementation.Base;
using Sources.Frameworks.MyGameCreator.Achivements.Presentation;
using Sources.InfrastructureInterfaces.Services.Repositories;
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

            _playerWallet = _entityRepository.
                Get<PlayerWallet>(ModelId.PlayerWallet);
            _achievement = _entityRepository
                .Get<Achievement>(ModelId.ScullsDiggerAchievement);

            _playerWallet.CoinsChanged += Execute;
        }

        public override void Execute()
        {
            if (_playerWallet.Coins < 100)
                return;
            
            _achievement.IsCompleted = true;
            _achievementView.Construct(_achievement);
            
            base.Execute();
        }

        public override void Destroy()
        {
            _playerWallet.CoinsChanged -= Execute;
        }
    }
}