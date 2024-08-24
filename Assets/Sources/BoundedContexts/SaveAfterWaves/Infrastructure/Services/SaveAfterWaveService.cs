using System;
using Sources.BoundedContexts.EnemySpawners.Domain.Models;
using Sources.BoundedContexts.Ids.Domain.Constant;
using Sources.Frameworks.GameServices.Loads.Services.Interfaces;
using Sources.Frameworks.GameServices.Repositories.Services.Interfaces;
using Sources.Frameworks.MVPPassiveView.Controllers.Interfaces.ControllerLifetimes;
using UnityEngine;

namespace Sources.BoundedContexts.SaveAfterWaves.Infrastructure.Services
{
    public class SaveAfterWaveService : IInitialize, IDestroy
    {
        private readonly IEntityRepository _entityRepository;
        private readonly ILoadService _loadService;
        private EnemySpawner _enemySpawner;

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
            _enemySpawner.WaveChanged += OnSave;
        }

        public void Destroy()
        {
            _enemySpawner.WaveChanged -= OnSave;
        }

        private void OnSave()
        {
            _loadService.Save(ModelId.ModelsIds);
            _loadService.Save(ModelId.AchievementModels);
        }
    }
}