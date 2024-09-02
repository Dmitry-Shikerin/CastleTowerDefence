using System;
using Doozy.Runtime.Signals;
using Sources.BoundedContexts.EnemySpawners.Domain.Models;
using Sources.BoundedContexts.GameCompleted.Infrastructure.Services.Interfaces;
using Sources.BoundedContexts.PlayerWallets.Domain.Models;
using Sources.Frameworks.DoozyWrappers.SignalBuses.Domain.Constants;
using Sources.Frameworks.GameServices.Loads.Domain.Constant;
using Sources.Frameworks.GameServices.Loads.Services.Interfaces;
using Sources.Frameworks.GameServices.Repositories.Services.Interfaces;
using Sources.Frameworks.YandexSdkFramework.Leaderboards.Services.Interfaces;

namespace Sources.BoundedContexts.GameCompleted.Infrastructure.Services.Implementation
{
    public class GameCompletedService : IGameCompletedService
    {
        private readonly IEntityRepository _entityRepository;
        private readonly ILoadService _loadService;
        private readonly ILeaderBoardScoreSetter _leaderBoardScoreSetter;

        private SignalStream _signalStream;
        private EnemySpawner _enemySpawner;
        private bool _isCompleted;

        public GameCompletedService(
            IEntityRepository entityRepository,
            ILoadService loadService,
            ILeaderBoardScoreSetter leaderBoardScoreSetter)
        {
            _entityRepository = entityRepository ?? throw new ArgumentNullException(nameof(entityRepository));
            _loadService = loadService ?? throw new ArgumentNullException(nameof(loadService));
            _leaderBoardScoreSetter = leaderBoardScoreSetter ?? throw new ArgumentNullException(nameof(leaderBoardScoreSetter));
        }

        public event Action GameCompleted;

        public void Initialize()
        {
            _enemySpawner = _entityRepository.Get<EnemySpawner>(ModelId.EnemySpawner) ?? 
                            throw new NullReferenceException(nameof(_enemySpawner));
            _signalStream = SignalStream.Get(StreamConst.Gameplay, StreamConst.GameCompleted);
            _enemySpawner.WaveKilled += OnCompleted;
        }

        public void Destroy() =>
            _enemySpawner.WaveKilled -= OnCompleted;

        private void OnCompleted()
        {
            if (_isCompleted)
                return;

            if (_enemySpawner.CurrentWaveNumber != 90) //todo поменять на константу
                return;

            int score = _entityRepository.Get<PlayerWallet>(ModelId.PlayerWallet).Coins;
            _leaderBoardScoreSetter.SetPlayerScore(score);
            _loadService.ClearAll();
            _signalStream.SendSignal(true);
            _isCompleted = true;
            GameCompleted?.Invoke();
        }
    }
}