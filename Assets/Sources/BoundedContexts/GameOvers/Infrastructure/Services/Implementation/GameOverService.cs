﻿using System;
using Doozy.Runtime.Signals;
using Sources.BoundedContexts.Bunkers.Domain;
using Sources.BoundedContexts.GameOvers.Infrastructure.Services.Interfaces;
using Sources.BoundedContexts.Ids.Domain.Constant;
using Sources.Frameworks.DoozyWrappers.SignalBuses.Domain.Constants;
using Sources.InfrastructureInterfaces.Services.Repositories;

namespace Sources.BoundedContexts.GameOvers.Infrastructure.Services.Implementation
{
    public class GameOverService : IGameOverService
    {
        private readonly IEntityRepository _entityRepository;
        private Bunker _bunker;
        private bool _isDeath;

        private SignalStream _signalStream;

        public GameOverService(IEntityRepository entityRepository)
        {
            _entityRepository = entityRepository ?? throw new ArgumentNullException(nameof(entityRepository));
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

            _signalStream.SendSignal(true);
            _isDeath = true;
        }
    }
}