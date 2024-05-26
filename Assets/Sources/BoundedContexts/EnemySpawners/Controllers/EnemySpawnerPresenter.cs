﻿using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Sources.BoundedContexts.BossEnemyView.Presentation;
using Sources.BoundedContexts.BossEnemyView.Presentation.Interfaces;
using Sources.BoundedContexts.Characters.PresentationInterfaces;
using Sources.BoundedContexts.Enemies.Infrastructure.Services.Spawners.Interfaces;
using Sources.BoundedContexts.Enemies.Presentation;
using Sources.BoundedContexts.EnemySpawners.Domain;
using Sources.BoundedContexts.EnemySpawners.Presentationinterfaces;
using Sources.BoundedContexts.KillEnemyCounters.Domain;
using Sources.BoundedContexts.SpawnPoints.Presentation;
using Sources.BoundedContexts.SpawnPoints.Presentation.Types;
using Sources.BoundedContexts.SpawnPoints.PresentationInterfaces;
using Sources.Controllers.Common;
using UnityEngine;

namespace Sources.BoundedContexts.EnemySpawners.Controllers
{
    public class EnemySpawnerPresenter : PresenterBase
    {
        private readonly EnemySpawner _enemySpawner;
        private readonly KillEnemyCounter _killEnemyCounter;
        private readonly IEnemySpawnerView _enemySpawnerView;
        private readonly IEnemySpawnService _enemySpawnService;

        private CancellationTokenSource _cancellationTokenSource;

        public EnemySpawnerPresenter(
            EnemySpawner enemySpawner,
            KillEnemyCounter killEnemyCounter,
            IEnemySpawnerView enemySpawnerView,
            IEnemySpawnService enemySpawnService)
        {
            _enemySpawner = enemySpawner ?? throw new ArgumentNullException(nameof(enemySpawner));
            _killEnemyCounter = killEnemyCounter ?? throw new ArgumentNullException(nameof(killEnemyCounter));
            _enemySpawnerView = enemySpawnerView ?? throw new ArgumentNullException(nameof(enemySpawnerView));
            _enemySpawnService = enemySpawnService ?? throw new ArgumentNullException(nameof(enemySpawnService));

            foreach (SpawnPoint spawnPoint in _enemySpawnerView.SpawnPoints)
            {
                if(spawnPoint == null)
                    throw new ArgumentNullException(nameof(spawnPoint));
                
                if(spawnPoint.Type != SpawnPointType.Enemy)
                    throw new ArgumentException(nameof(spawnPoint.Type));
            }
        }

        public override void Enable()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            Spawn(_cancellationTokenSource.Token);
        }

        public override void Disable()
        {
            _cancellationTokenSource.Cancel();
        }

        private async void Spawn(CancellationToken cancellationToken)
        {
            try
            {
                while (_cancellationTokenSource.IsCancellationRequested == false)
                {
                    foreach (ISpawnPoint spawnPoint in _enemySpawnerView.SpawnPoints)
                    {
                        _enemySpawner.SetCurrentWave(_killEnemyCounter.KillZombies);
                        SpawnEnemy(spawnPoint.Position, _enemySpawnerView.CharacterView);
                        SpawnBoss(spawnPoint.Position, _enemySpawnerView.CharacterView);
                        
                        await _enemySpawner.WaitWave(_killEnemyCounter, cancellationToken);
                        await UniTask.Delay(
                            TimeSpan.FromSeconds(
                                _enemySpawner.SpawnDelays[_enemySpawner.CurrentWave]),
                            cancellationToken: cancellationToken);
                    }
                }
            }
            catch (OperationCanceledException)
            {
            }
        }
        
        private void SpawnEnemy(Vector3 position, ICharacterView characterView)
        {
            if (_enemySpawner.IsSpawnEnemy == false)
                  return;
            
            IEnemyView enemyView = _enemySpawnService.Spawn(_killEnemyCounter, position);
            enemyView.SetCharacterHealth(characterView.HealthView);
            enemyView.SetTargetFollow(characterView.HealthView);

            _enemySpawner.SpawnedEnemies++;
        }

        private void SpawnBoss(Vector3 position, ICharacterView characterView)
        {
            if (_enemySpawner.IsSpawnBoss == false)
                return;
            
            // IBossEnemyView bossEnemyView = _bossEnemySpawnService.Spawn(_killEnemyCounter, position);
            // bossEnemyView.SetCharacterHealth(characterView.CharacterHealthView);
            // bossEnemyView.SetTargetFollow(characterView.CharacterMovementView);

            _enemySpawner.SpawnedBosses++;
            _cancellationTokenSource.Cancel();
        }
    }
}