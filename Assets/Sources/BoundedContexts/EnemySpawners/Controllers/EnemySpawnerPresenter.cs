using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Sources.BoundedContexts.CharacterMelees.Presentation.Interfaces;
using Sources.BoundedContexts.Enemies.Infrastructure.Services.Spawners.Interfaces;
using Sources.BoundedContexts.Enemies.Presentation;
using Sources.BoundedContexts.Enemies.PresentationInterfaces;
using Sources.BoundedContexts.EnemySpawners.Domain;
using Sources.BoundedContexts.EnemySpawners.Domain.Models;
using Sources.BoundedContexts.EnemySpawners.Presentation.Implementation;
using Sources.BoundedContexts.EnemySpawners.Presentation.Interfaces;
using Sources.BoundedContexts.KillEnemyCounters.Domain;
using Sources.BoundedContexts.SpawnPoints.Presentation;
using Sources.BoundedContexts.SpawnPoints.Presentation.Implementation;
using Sources.BoundedContexts.SpawnPoints.Presentation.Implementation.Types;
using Sources.Frameworks.MVPPassiveView.Controllers.Implementation;
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

            foreach (IEnemySpawnPoint spawnPoint in _enemySpawnerView.SpawnPoints)
            {
                if(spawnPoint == null)
                    throw new ArgumentNullException(nameof(spawnPoint));
                
                if(spawnPoint.Type != SpawnPointType.Enemy)
                    throw new ArgumentException(nameof(spawnPoint.Type));
                
                if(spawnPoint.CharacterMeleeSpawnPoint == null)
                    throw new ArgumentNullException(nameof(spawnPoint.CharacterMeleeSpawnPoint));
                
                if(spawnPoint.CharacterRangedSpawnPoint == null)
                    throw new ArgumentNullException(nameof(spawnPoint.CharacterRangedSpawnPoint));
            }
        }

        public override void Enable()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            // Spawn(_cancellationTokenSource.Token);
            SpawnEnemy(_enemySpawnerView.SpawnPoints[0]);
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
                    foreach (IEnemySpawnPoint spawnPoint in _enemySpawnerView.SpawnPoints)
                    {
                        // _enemySpawner.SetCurrentWave(_killEnemyCounter.KillZombies);
                        SpawnEnemy(spawnPoint);
                        // SpawnBoss(spawnPoint.Position, _enemySpawnerView.CharacterMeleeView);
                        
                        // await _enemySpawner.WaitWave(_killEnemyCounter, cancellationToken);
                        await UniTask.Delay(TimeSpan.FromSeconds(1), cancellationToken: cancellationToken);
                        // await UniTask.Delay(
                        //     TimeSpan.FromSeconds(
                        //         _enemySpawner.SpawnDelays[_enemySpawner.CurrentWave]),
                        //     cancellationToken: cancellationToken);
                    }
                }
            }
            catch (OperationCanceledException)
            {
            }
        }
        
        private void SpawnEnemy(IEnemySpawnPoint spawnPoint)
        {
            if (_enemySpawner.IsSpawnEnemy == false)
                  return;
            
            IEnemyView enemyView = _enemySpawnService.Spawn(_killEnemyCounter, spawnPoint.Position);
            enemyView.SetTargetPoint(_enemySpawnerView.TargetPoint);
            enemyView.SetCharacterMeleePoint(spawnPoint.CharacterMeleeSpawnPoint);
            enemyView.SetCharacterRangePoint(spawnPoint.CharacterRangedSpawnPoint);
            // enemyView.SetCharacterHealth(characterMeleeView.HealthView);
            // enemyView.SetTargetFollow(characterMeleeView.HealthView);

            _enemySpawner.SpawnedEnemies++;
        }

        private void SpawnBoss(Vector3 position, ICharacterMeleeView characterMeleeView)
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