using System;
using Doozy.Runtime.Signals;
using Sources.BoundedContexts.EnemySpawners.Domain.Models;
using Sources.BoundedContexts.GameCompleteds.Infrastructure.Services.Interfaces;
using Sources.BoundedContexts.Ids.Domain.Constant;
using Sources.Frameworks.DoozyWrappers.SignalBuses.Domain.Constants;
using Sources.InfrastructureInterfaces.Services.Repositories;

namespace Sources.BoundedContexts.GameCompleteds.Infrastructure.Services.Implementation
{
    public class GameCompletedService : IGameCompletedService
    {
        private readonly IEntityRepository _entityRepository;

        private SignalStream _signalStream;
        private EnemySpawner _enemySpawner;
        private bool _isCompleted;

        public GameCompletedService(IEntityRepository entityRepository)
        {
            _entityRepository = entityRepository ?? throw new ArgumentNullException(nameof(entityRepository));
        }

        public void Initialize()
        {
            _enemySpawner = _entityRepository.Get<EnemySpawner>(ModelId.EnemySpawner) ?? 
                            throw new NullReferenceException(nameof(_enemySpawner));
            _signalStream = SignalStream.Get(StreamConst.Gameplay, StreamConst.GameCompleted);
            
            _enemySpawner.WaveKilled += OnCompleted;
        }

        public void Destroy()
        {
            _enemySpawner.WaveKilled -= OnCompleted;
        }

        private void OnCompleted()
        {
            if (_isCompleted)
                return;

            if (_enemySpawner.CurrentWaveNumber != 99) //todo поменять на константу
                return;

            _signalStream.SendSignal(true);
            _isCompleted = true;
        }
    }
}