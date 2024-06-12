using System;
using System.Collections.Generic;
using Sources.BoundedContexts.EnemySpawners.Domain.Configs;
using Sources.Frameworks.Domain.Interfaces.Entities;

namespace Sources.BoundedContexts.EnemySpawners.Domain.Models
{
    public class EnemySpawner : IEntity
    {
        private readonly EnemySpawnerConfig _enemySpawnerConfig;
        private int _currentWaveNumber;
        private int _spawnedAllEnemies;

        public EnemySpawner(EnemySpawnerConfig enemySpawnerConfig, string id)
        {
            if (enemySpawnerConfig == null) 
                throw new ArgumentNullException(nameof(enemySpawnerConfig));
            
            _enemySpawnerConfig = enemySpawnerConfig;

            Waves = enemySpawnerConfig.Waves;
            Id = id;
        }

        public event Action WaveChanged;
        public event Action SpawnedAllEnemiesChanged;

        public string Id { get; }
        public Type Type => GetType();
        public IReadOnlyList<EnemySpawnerWave> Waves { get; private set; }
        public EnemySpawnerWave CurrentWave => Waves[CurrentWaveNumber];
        
        public int EnemyHealth => 
            _enemySpawnerConfig.StartEnemyHealth + 
            _enemySpawnerConfig.AddedEnemyHealth * 
            CurrentWaveNumber;

        public int EnemyAttackPower => 
            _enemySpawnerConfig.StartEnemyAttackPower + 
            _enemySpawnerConfig.AddedEnemyAttackPower * 
            CurrentWaveNumber;

        public int KamikazeHealth =>
            _enemySpawnerConfig.StartKamikazeHealth + 
            _enemySpawnerConfig.AddedKamikazeHealth * 
            CurrentWaveNumber;

        public int KamikazeAttackPower => 
            _enemySpawnerConfig.StartKamikazeAttackPower + 
            _enemySpawnerConfig.AddedKamikazeAttackPower * 
            CurrentWaveNumber;

        public int KamikazeMassAttackPower => 
            _enemySpawnerConfig.StartKamikazeMassAttackPower + 
            _enemySpawnerConfig.AddedKamikazeMassAttackPower * 
            CurrentWaveNumber;

        public int BossMassAttackPower => 
            _enemySpawnerConfig.StartBossMassAttackPower + 
            _enemySpawnerConfig.AddedBossMassAttackPower * 
            CurrentWaveNumber;

        public int BossAttackPower => 
            _enemySpawnerConfig.StartBossAttackPower + 
            _enemySpawnerConfig.AddedBossAttackPower * 
            CurrentWaveNumber;

        public float BossHealth => 
            _enemySpawnerConfig.StartBossHealth + 
            _enemySpawnerConfig.AddedBossHealth * 
            CurrentWaveNumber;

        public int CurrentWaveNumber
        {
            get => _currentWaveNumber;
            set
            {
                _currentWaveNumber = value;
                WaveChanged?.Invoke();
            }
        }

        public int SpawnedAllEnemies
        {
            get => _spawnedAllEnemies;
            set
            {
                _spawnedAllEnemies = value;
                SpawnedAllEnemiesChanged?.Invoke();
            }
        }

        public int SpawnedEnemiesInCurrentWave { get; set; }
        public int SpawnedBossesInCurrentWave { get; set; }
        public int SpawnedKamikazeInCurrentWave { get; set; }

        public void ClearSpawnedEnemies()
        {
            SpawnedKamikazeInCurrentWave = 0;
            SpawnedBossesInCurrentWave = 0;
            SpawnedEnemiesInCurrentWave = 0;
        }
    }
}