using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Sources.BoundedContexts.CharacterMelees.Presentation.Interfaces;
using Sources.BoundedContexts.Enemies.Infrastructure.Services.Spawners.Interfaces;
using Sources.BoundedContexts.Enemies.PresentationInterfaces;
using Sources.BoundedContexts.EnemyBosses.Infrastructure.Services.Spawners.Interfaces;
using Sources.BoundedContexts.EnemyBosses.Presentation.Interfaces;
using Sources.BoundedContexts.EnemyKamikazes.Infrastructure.Services.Spawners.Interfaces;
using Sources.BoundedContexts.EnemySpawners.Domain.Models;
using Sources.BoundedContexts.EnemySpawners.Presentation.Interfaces;
using Sources.BoundedContexts.KillEnemyCounters.Domain;
using Sources.BoundedContexts.SpawnPoints.Presentation.Implementation.Types;
using Sources.Frameworks.MVPPassiveView.Controllers.Implementation;
using Sources.Utils.Extensions;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Sources.BoundedContexts.EnemySpawners.Controllers
{
    public class EnemySpawnerPresenter : PresenterBase
    {
        private readonly EnemySpawner _enemySpawner;
        private readonly KillEnemyCounter _killEnemyCounter;
        private readonly IEnemySpawnerView _view;
        private readonly IEnemySpawnService _enemySpawnService;
        private readonly IEnemyKamikazeSpawnService _enemyKamikazeSpawnService;
        private readonly IEnemyBossSpawnService _enemyBossSpawnService;

        private CancellationTokenSource _cancellationTokenSource;

        public EnemySpawnerPresenter(
            EnemySpawner enemySpawner,
            KillEnemyCounter killEnemyCounter,
            IEnemySpawnerView enemySpawnerView,
            IEnemySpawnService enemySpawnService,
            IEnemyKamikazeSpawnService enemyKamikazeSpawnService,
            IEnemyBossSpawnService enemyBossSpawnService)
        {
            _enemySpawner = enemySpawner ?? throw new ArgumentNullException(nameof(enemySpawner));
            _killEnemyCounter = killEnemyCounter ?? throw new ArgumentNullException(nameof(killEnemyCounter));
            _view = enemySpawnerView ?? throw new ArgumentNullException(nameof(enemySpawnerView));
            _enemySpawnService = enemySpawnService ?? throw new ArgumentNullException(nameof(enemySpawnService));
            _enemyKamikazeSpawnService = enemyKamikazeSpawnService ?? 
                                         throw new ArgumentNullException(nameof(enemyKamikazeSpawnService));
            _enemyBossSpawnService = enemyBossSpawnService ??
                                     throw new ArgumentNullException(nameof(enemyBossSpawnService));

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
            Spawn(_cancellationTokenSource.Token);
            // SpawnEnemy(_view.SpawnPoints[0]);
        }

        public override void Disable()
        {
            _cancellationTokenSource.Cancel();
        }

        private async void Spawn(CancellationToken cancellationToken)
        {
            try
            {
                    int startWave = _enemySpawner.CurrentWave;

                    for (int i = startWave; i < _enemySpawner.Waves.Count; i++)
                    {
                        _enemySpawner.SpawnedEnemiesInCurrentWave = 0;
                        
                        for (int j = 0; j < _enemySpawner.Waves[i].EnemyCount; j++)
                        {
                            int randomSpawnPoint = Random.Range(0, _view.SpawnPoints.Count);
                            // SpawnEnemy(_view.SpawnPoints[randomSpawnPoint]);
                            // SpawnEnemyKamikaze(_view.SpawnPoints[randomSpawnPoint]);
                            SpawnBoss(_view.SpawnPoints[randomSpawnPoint]);

                            await UniTask.Delay(TimeSpan.FromSeconds(
                                    _enemySpawner.Waves[i].SpawnDelay),
                                cancellationToken: cancellationToken);
                        }

                        _enemySpawner.CurrentWave++;
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
            enemyView.SetBunkerView(_view.BunkerView);
            enemyView.SetCharacterMeleePoint(spawnPoint.CharacterMeleeSpawnPoint);
            enemyView.SetCharacterRangePoint(spawnPoint.CharacterRangedSpawnPoint);

            _enemySpawner.SpawnedEnemiesInCurrentWave++;
        }

        private void SpawnEnemyKamikaze(IEnemySpawnPoint spawnPoint)
        {
            var enemyView = _enemyKamikazeSpawnService.Spawn(_killEnemyCounter, spawnPoint.Position);
            enemyView.SetBunkerView(_view.BunkerView);
            enemyView.SetCharacterMeleePoint(spawnPoint.CharacterMeleeSpawnPoint);
        }

        private void SpawnBoss(IEnemySpawnPoint spawnPoint)
        {
            // if (_enemySpawner.IsSpawnBoss == false)
            //     return;
            //
            IEnemyBossView bossEnemyView = _enemyBossSpawnService.Spawn(_killEnemyCounter, spawnPoint.Position);
            bossEnemyView.SetBunkerView(_view.BunkerView);
            bossEnemyView.SetCharacterMeleePoint(spawnPoint.CharacterMeleeSpawnPoint);

            _enemySpawner.SpawnedBosses++;
            _cancellationTokenSource.Cancel();
        }
    }
}