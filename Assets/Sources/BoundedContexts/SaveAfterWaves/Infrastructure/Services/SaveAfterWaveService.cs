using System;
using Doozy.Runtime.Signals;
using Sources.BoundedContexts.EnemySpawners.Domain.Models;
using Sources.Frameworks.DoozyWrappers.SignalBuses.Domain.Constants;
using Sources.Frameworks.GameServices.Loads.Domain.Constant;
using Sources.Frameworks.GameServices.Loads.Services.Interfaces;
using Sources.Frameworks.GameServices.Repositories.Services.Interfaces;
using Sources.Frameworks.MVPPassiveView.Controllers.Interfaces.ControllerLifetimes;

namespace Sources.BoundedContexts.SaveAfterWaves.Infrastructure.Services
{
    public class SaveAfterWaveService : IInitialize, IDestroy
    {
        private readonly IEntityRepository _entityRepository;
        private readonly ILoadService _loadService;
        private EnemySpawner _enemySpawner;
        private SignalStream _stream;

        public SaveAfterWaveService(
            IEntityRepository entityRepository,
            ILoadService loadService)
        {
            _entityRepository = entityRepository ?? throw new ArgumentNullException(nameof(entityRepository));
            _loadService = loadService ?? throw new ArgumentNullException(nameof(loadService));
        }

        public void Initialize()
        {
            _enemySpawner = _entityRepository.Get<EnemySpawner>(ModelId.EnemySpawner);
            _stream = SignalStream.Get(StreamConst.Gameplay, StreamConst.Saving);
            _enemySpawner.WaveChanged += OnSave;
        }

        public void Destroy() =>
            _enemySpawner.WaveChanged -= OnSave;

        private void OnSave()
        {
            _stream.SendSignal(true);
            _loadService.SaveAll();
        }
    }
}