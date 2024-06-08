using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Sources.BoundedContexts.EnemySpawners.Domain.Configs;
using Sources.BoundedContexts.KillEnemyCounters.Domain;

namespace Sources.BoundedContexts.EnemySpawners.Domain.Models
{
    public class EnemySpawner
    {
        private int _currentWave;

        public EnemySpawner(EnemySpawnerConfigContainer enemySpawnerConfig)
        {
            if (enemySpawnerConfig == null) 
                throw new ArgumentNullException(nameof(enemySpawnerConfig));
            
            Waves = enemySpawnerConfig.Waves;
        }

        public event Action WaveChanged;

        public IReadOnlyList<EnemySpawnerWave> Waves { get; private set; }

        public int CurrentWave
        {
            get => _currentWave;
            set
            {
                _currentWave = value;
                WaveChanged?.Invoke();
            }
        }
        
        public bool IsSpawnEnemy { get; set; } = true;
        public int SpawnedEnemies { get; set; }
        public bool IsSpawnBoss { get; set; }
        public int SpawnedBosses { get; set; }

        public UniTask WaitWave(KillEnemyCounter killEnemyCounter, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public void SetCurrentWave(int killZombies)
        {
            throw new System.NotImplementedException();
        }
    }
}