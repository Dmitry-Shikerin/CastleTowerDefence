﻿using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Sources.BoundedContexts.Enemies.Infrastructure.Factories.Views.Implementation;
using Sources.BoundedContexts.Enemies.PresentationInterfaces;
using Sources.BoundedContexts.EnemyBosses.Infrastructure.Factories.Views;
using Sources.BoundedContexts.EnemyBosses.Presentation.Interfaces;
using Sources.BoundedContexts.EnemyKamikazes.Infrastructure.Factories.Views.Implementation;
using Sources.BoundedContexts.EnemyKamikazes.Presentations.Interfaces;
using Sources.BoundedContexts.EnemySpawners.Domain.Models;
using Sources.BoundedContexts.EnemySpawners.Presentation.Interfaces;
using Sources.BoundedContexts.Ids.Domain.Constant;
using Sources.BoundedContexts.KillEnemyCounters.Domain.Models.Implementation;
using Sources.BoundedContexts.SpawnPoints.Presentation.Implementation.Types;
using Sources.BoundedContexts.Tutorials.Domain.Models;
using Sources.Frameworks.GameServices.Repositories.Services.Interfaces;
using Sources.Frameworks.MVPPassiveView.Controllers.Implementation;
using Sources.Utils.Extentions;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Sources.BoundedContexts.EnemySpawners.Controllers
{
    public class EnemySpawnerPresenter : PresenterBase
    {
        private readonly EnemySpawner _enemySpawner;
        private readonly KillEnemyCounter _killEnemyCounter;
        private readonly Tutorial _tutorial;
        private readonly IEnemySpawnerView _view;
        private readonly EnemyViewFactory _enemyViewFactory;
        private readonly EnemyKamikazeViewFactory _enemyKamikazeViewFactory;

        private readonly EnemyBossViewFactory _enemyBossViewFactory;

        private CancellationTokenSource _cancellationTokenSource;

        public EnemySpawnerPresenter(
            IEntityRepository entityRepository,
            IEnemySpawnerView enemySpawnerView,
            EnemyViewFactory enemyViewFactory,
            EnemyKamikazeViewFactory enemyKamikazeViewFactory,
            EnemyBossViewFactory enemyBossViewFactory)
        {
            _enemySpawner = entityRepository.Get<EnemySpawner>(ModelId.EnemySpawner);
            _killEnemyCounter = entityRepository.Get<KillEnemyCounter>(ModelId.KillEnemyCounter);
            _tutorial = entityRepository.Get<Tutorial>(ModelId.Tutorial);
            _view = enemySpawnerView ?? throw new ArgumentNullException(nameof(enemySpawnerView));
            _enemyViewFactory = enemyViewFactory ?? throw new ArgumentNullException(nameof(enemyViewFactory));
            _enemyKamikazeViewFactory = enemyKamikazeViewFactory ??
                                        throw new ArgumentNullException(nameof(enemyKamikazeViewFactory));
            _enemyBossViewFactory = enemyBossViewFactory ??
                                    throw new ArgumentNullException(nameof(enemyBossViewFactory));

            foreach (IEnemySpawnPoint spawnPoint in _view.SpawnPoints)
            {
                if (spawnPoint == null)
                    throw new ArgumentNullException(nameof(spawnPoint));

                if (spawnPoint.Type != SpawnPointType.Enemy)
                    throw new ArgumentException(nameof(spawnPoint.Type));

                if (spawnPoint.CharacterMeleeSpawnPoint == null)
                    throw new ArgumentNullException(nameof(spawnPoint.CharacterMeleeSpawnPoint));

                if (spawnPoint.CharacterRangedSpawnPoint == null)
                    throw new ArgumentNullException(nameof(spawnPoint.CharacterRangedSpawnPoint));
            }
        }

        public override void Enable()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            _killEnemyCounter.KillZombiesCountChanged += OnKillZombiesCountChanged;
            OnStartSpawn();
            _tutorial.OnCompleted += OnStartSpawn;
        }

        public override void Disable()
        {
            _killEnemyCounter.KillZombiesCountChanged -= OnKillZombiesCountChanged;
            _tutorial.OnCompleted -= OnStartSpawn;
            _cancellationTokenSource.Cancel();
        }

        private void OnStartSpawn()
        {
            if (_tutorial.HasCompleted == false)
                return;

            Spawn(_cancellationTokenSource.Token);
        }

        private void OnKillZombiesCountChanged()
        {
            if (_killEnemyCounter.KillZombies < _enemySpawner.GetSumEnemiesInWave(_enemySpawner.KilledWaves))
                return;

            _enemySpawner.KilledWaves++;
        }

        private async void Spawn(CancellationToken cancellationToken)
        {
            try
            {
                int startWave = _enemySpawner.CurrentWaveNumber;

                for (int i = startWave; i < _enemySpawner.Waves.Count; i++)
                {
                    _enemySpawner.ClearSpawnedEnemies();

                    for (int j = 0; j < _enemySpawner.Waves[i].BossesCount; j++)
                    {
                        int randomSpawnPoint = Random.Range(0, _view.SpawnPoints.Count);
                        SpawnBoss(_view.SpawnPoints[randomSpawnPoint]);

                        await UniTask.Delay(TimeSpan.FromSeconds(
                                _enemySpawner.Waves[i].SpawnDelay),
                            cancellationToken: cancellationToken);
                    }

                    for (int j = 0; j < _enemySpawner.Waves[i].EnemyCount; j++)
                    {
                        int randomSpawnPoint = Random.Range(0, _view.SpawnPoints.Count);
                        SpawnEnemy(_view.SpawnPoints[randomSpawnPoint]);

                        int percent =
                            _enemySpawner.SpawnedEnemiesInCurrentWave.IntToPercent(
                                _enemySpawner.Waves[i].EnemyCount);

                        await UniTask.Delay(TimeSpan.FromSeconds(
                                _enemySpawner.Waves[i].SpawnDelay),
                            cancellationToken: cancellationToken);

                        if (_enemySpawner.SpawnedKamikazeInCurrentWave ==
                            _enemySpawner.Waves[i].KamikazeEnemyCount)
                            continue;

                        if (percent >= 50)
                        {
                            for (int x = 0; x < _enemySpawner.Waves[i].KamikazeEnemyCount; x++)
                            {
                                int randomSpawnPoint2 = Random.Range(0, _view.SpawnPoints.Count);
                                SpawnEnemyKamikaze(_view.SpawnPoints[randomSpawnPoint2]);

                                await UniTask.Delay(TimeSpan.FromSeconds(
                                        _enemySpawner.Waves[i].SpawnDelay),
                                    cancellationToken: cancellationToken);
                            }
                        }
                    }

                    _enemySpawner.CurrentWaveNumber++;
                }
            }
            catch (OperationCanceledException)
            {
            }
        }

        private void SpawnEnemy(IEnemySpawnPoint spawnPoint)
        {
            IEnemyView enemyView = _enemyViewFactory.Create(_enemySpawner, spawnPoint.Position);
            enemyView.SetBunkerView(_view.BunkerView);
            enemyView.SetCharacterMeleePoint(spawnPoint.CharacterMeleeSpawnPoint);

            _enemySpawner.SpawnedEnemiesInCurrentWave++;
            _enemySpawner.SpawnedAllEnemies++;
        }

        private void SpawnEnemyKamikaze(IEnemySpawnPoint spawnPoint)
        {
            IEnemyKamikazeView enemyView = _enemyKamikazeViewFactory.Create(_enemySpawner, spawnPoint.Position);
            enemyView.SetBunkerView(_view.BunkerView);
            enemyView.SetCharacterMeleePoint(spawnPoint.CharacterMeleeSpawnPoint);

            _enemySpawner.SpawnedKamikazeInCurrentWave++;
            _enemySpawner.SpawnedAllEnemies++;
        }

        private void SpawnBoss(IEnemySpawnPoint spawnPoint)
        {
            IEnemyBossView bossEnemyView = _enemyBossViewFactory.Create(_enemySpawner, spawnPoint.Position);
            bossEnemyView.SetBunkerView(_view.BunkerView);
            bossEnemyView.SetCharacterMeleePoint(spawnPoint.CharacterMeleeSpawnPoint);

            _enemySpawner.SpawnedBossesInCurrentWave++;
            _enemySpawner.SpawnedAllEnemies++;
        }
    }
}