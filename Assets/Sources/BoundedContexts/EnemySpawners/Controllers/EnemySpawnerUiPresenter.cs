using System;
using Sources.BoundedContexts.EnemySpawners.Domain.Models;
using Sources.BoundedContexts.EnemySpawners.Presentation.Interfaces;
using Sources.Frameworks.GameServices.Loads.Domain.Constant;
using Sources.Frameworks.GameServices.Repositories.Services.Interfaces;
using Sources.Frameworks.MVPPassiveView.Controllers.Implementation;
using Sources.Frameworks.Utils.Extensions;

namespace Sources.BoundedContexts.EnemySpawners.Controllers
{
    public class EnemySpawnerUiPresenter : PresenterBase
    {
        private readonly EnemySpawner _enemySpawner;
        private readonly IEnemySpawnerUi _view;

        public EnemySpawnerUiPresenter(IEntityRepository entityRepository, IEnemySpawnerUi view)
        {
            if (entityRepository == null) 
                throw new ArgumentNullException(nameof(entityRepository));
            
            _enemySpawner = entityRepository.Get<EnemySpawner>(ModelId.EnemySpawner);
            _view = view ?? throw new ArgumentNullException(nameof(view));
        }

        public override void Enable()
        {
            SetWaveProgress();
            OnSpawnedEnemiesInCurrentWaveChanged();
            _enemySpawner.WaveChanged += OnWaveChanged;
            _enemySpawner.WaveKilled += OnWaveKilled;
            _enemySpawner.SpawnedAllEnemiesChanged += OnSpawnedEnemiesInCurrentWaveChanged;
        }

        public override void Disable()
        {
            _enemySpawner.WaveChanged -= OnWaveChanged;
            _enemySpawner.WaveKilled -= OnWaveKilled;
            _enemySpawner.SpawnedAllEnemiesChanged -= OnSpawnedEnemiesInCurrentWaveChanged;
        }

        private void OnWaveChanged() => 
            SetWaveProgress();

        private void OnWaveKilled() => 
            _view.PopUpAnimator.Play();

        private void OnSpawnedEnemiesInCurrentWaveChanged()
        {
            int percent =
                _enemySpawner.SpawnedEnemiesInCurrentWave.IntToPercent(
                    _enemySpawner.CurrentWave.SumEnemies);

            float unitPercent = percent.IntPercentToUnitPercent();

            _view.SpawnerProgressSlider.value = unitPercent;
        }

        private void SetWaveProgress()
        {
            _view.CurrentWaveText.SetText(_enemySpawner.CurrentWaveNumber.ToString());
            _view.SpawnerProgressSlider.value = 0;
        }
    }
}