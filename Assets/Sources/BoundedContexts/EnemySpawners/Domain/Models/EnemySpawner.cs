using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Sources.BoundedContexts.EnemySpawners.Domain.Data;
using Sources.Frameworks.Domain.Interfaces.Entities;

namespace Sources.BoundedContexts.EnemySpawners.Domain.Models
{
    public class EnemySpawner : IEntity
    {
        private int _currentWaveNumber = 1;
        private int _spawnedAllEnemies;
        private int _killedWaves;
        
        public event Action WaveChanged;
        public event Action SpawnedAllEnemiesChanged;
        public event Action WaveKilled;

        public string Id { get; set; }
        public Type Type => GetType();
        public List<RuntimeEnemySpawnerWave> Waves { get; set; }
        public List<RuntimeEnemySpawnStrategy> SpawnStrategies { get; set; }
        public RuntimeEnemySpawnerConfig Config { get; set; }
        public Type LastSpawnedEnemyType { get; set; }

        [JsonIgnore]
        public RuntimeEnemySpawnerWave CurrentWave => Waves[CurrentWaveNumber];
        [JsonIgnore]
        public int EnemyHealth => 
            Config.StartEnemyHealth + 
            Config.AddedEnemyHealth * 
            CurrentWaveNumber;
        [JsonIgnore]
        public int EnemyAttackPower => 
            Config.StartEnemyAttackPower + 
            Config.AddedEnemyAttackPower * 
            CurrentWaveNumber;
        [JsonIgnore]
        public int KamikazeHealth =>
            Config.StartKamikazeHealth + 
            Config.AddedKamikazeHealth * 
            CurrentWaveNumber;
        [JsonIgnore]
        public int KamikazeAttackPower => 
            Config.StartKamikazeAttackPower + 
            Config.AddedKamikazeAttackPower * 
            CurrentWaveNumber;
        [JsonIgnore]
        public int KamikazeMassAttackPower => 
            Config.StartKamikazeMassAttackPower + 
            Config.AddedKamikazeMassAttackPower * 
            CurrentWaveNumber;
        [JsonIgnore]
        public int BossMassAttackPower => 
            Config.StartBossMassAttackPower + 
            Config.AddedBossMassAttackPower * 
            CurrentWaveNumber;
        [JsonIgnore]
        public int BossAttackPower => 
            Config.StartBossAttackPower + 
            Config.AddedBossAttackPower * 
            CurrentWaveNumber;
        [JsonIgnore]
        public float BossHealth => 
            Config.StartBossHealth + 
            Config.AddedBossHealth * 
            CurrentWaveNumber;
        [JsonIgnore]
        public bool CanSpawnBoss => SpawnedBossesInCurrentWave < CurrentWave.BossesCount;
        [JsonIgnore]
        public bool CanSpawnKamikaze => SpawnedKamikazeInCurrentWave < CurrentWave.KamikazeEnemyCount;
        [JsonIgnore]
        public bool CanSpawnEnemy => SpawnedEnemiesInCurrentWave < CurrentWave.SumEnemies;

        public int KilledWaves
        {
            get => _killedWaves;
            set
            {
                _killedWaves = value;
                WaveKilled?.Invoke();
            }
        }

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
        
        public int GetSumEnemiesInWave(int waveNumber)
        {
            int sum = 0;
            
            for (int i = 0; i < waveNumber; i++)
                sum += Waves[i].SumEnemies;
            
            return sum;
        }
    }
}