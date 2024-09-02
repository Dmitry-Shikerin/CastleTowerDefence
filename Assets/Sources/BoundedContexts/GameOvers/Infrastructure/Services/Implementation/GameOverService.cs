using System;
using Doozy.Runtime.Signals;
using Sources.BoundedContexts.Bunkers.Domain;
using Sources.BoundedContexts.GameOvers.Infrastructure.Services.Interfaces;
using Sources.BoundedContexts.PlayerWallets.Domain.Models;
using Sources.Frameworks.DoozyWrappers.SignalBuses.Domain.Constants;
using Sources.Frameworks.GameServices.Loads.Domain.Constant;
using Sources.Frameworks.GameServices.Loads.Services.Interfaces;
using Sources.Frameworks.GameServices.Repositories.Services.Interfaces;
using Sources.Frameworks.YandexSdkFramework.Leaderboards.Services.Interfaces;

namespace Sources.BoundedContexts.GameOvers.Infrastructure.Services.Implementation
{
    public class GameOverService : IGameOverService
    {
        private readonly IEntityRepository _entityRepository;
        private readonly ILoadService _loadService;
        private readonly ILeaderBoardScoreSetter _leaderBoardScoreSetter;
        private Bunker _bunker;
        private bool _isDeath;

        private SignalStream _signalStream;

        public GameOverService(
            IEntityRepository entityRepository,
            ILoadService loadService,
            ILeaderBoardScoreSetter leaderBoardScoreSetter)
        {
            _entityRepository = entityRepository ?? throw new ArgumentNullException(nameof(entityRepository));
            _loadService = loadService ?? throw new ArgumentNullException(nameof(loadService));
            _leaderBoardScoreSetter = leaderBoardScoreSetter ?? throw new ArgumentNullException(nameof(leaderBoardScoreSetter));
        }

        public void Initialize()
        {
            _bunker = _entityRepository.Get<Bunker>(ModelId.Bunker) ?? 
                      throw new NullReferenceException(nameof(_bunker));
            _bunker.Death += OnDeath;
            _signalStream = SignalStream.Get(StreamConst.Gameplay, StreamConst.GameOver);
        }

        public void Destroy()
        {
            _bunker.Death -= OnDeath;
        }

        private void OnDeath()
        {
            if (_isDeath)
                return;

            int score = _entityRepository.Get<PlayerWallet>(ModelId.PlayerWallet).Coins;
            _leaderBoardScoreSetter.SetPlayerScore(score);
            _loadService.ClearAll();
            _signalStream.SendSignal(true);
            _isDeath = true;
        }
    }
}