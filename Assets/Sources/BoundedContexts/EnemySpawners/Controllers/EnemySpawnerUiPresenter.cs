﻿using System;
using Sources.BoundedContexts.EnemySpawners.Domain.Models;
using Sources.BoundedContexts.EnemySpawners.Presentation.Interfaces;
using Sources.Frameworks.MVPPassiveView.Controllers.Implementation;
using Sources.Utils.Extentions;

namespace Sources.BoundedContexts.EnemySpawners.Controllers
{
    public class EnemySpawnerUiPresenter : PresenterBase
    {
        private readonly EnemySpawner _enemySpawner;
        private readonly IEnemySpawnerUi _view;

        public EnemySpawnerUiPresenter(EnemySpawner enemySpawner, IEnemySpawnerUi view)
        {
            _enemySpawner = enemySpawner ?? throw new ArgumentNullException(nameof(enemySpawner));
            _view = view ?? throw new ArgumentNullException(nameof(view));
        }

        public override void Enable()
        {
            OnWaveChanged();
            OnSpawnedEnemiesInCurrentWaveChanged();
            _enemySpawner.WaveChanged += OnWaveChanged;
            _enemySpawner.SpawnedEnemiesInCurrentWaveChanged += OnSpawnedEnemiesInCurrentWaveChanged;
        }

        public override void Disable()
        {
            _enemySpawner.WaveChanged -= OnWaveChanged;
            _enemySpawner.SpawnedEnemiesInCurrentWaveChanged -= OnSpawnedEnemiesInCurrentWaveChanged;
        }

        private void OnWaveChanged()
        {
            _view.CurrentWaveText.SetText(_enemySpawner.CurrentWave.ToString());   
        }

        private void OnSpawnedEnemiesInCurrentWaveChanged()
        {
            int percent =
                _enemySpawner.SpawnedEnemiesInCurrentWave.IntToPercent(
                    _enemySpawner.Waves[_enemySpawner.CurrentWave].EnemyCount);

            float unitPercent = percent.IntPercentToUnitPercent();

            _view.SpawnerProgressSlider.value = unitPercent;
        }
    }
}