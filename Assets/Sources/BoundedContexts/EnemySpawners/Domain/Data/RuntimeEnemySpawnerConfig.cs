using System.Collections.Generic;
using Sources.BoundedContexts.EnemySpawners.Domain.Configs;

namespace Sources.BoundedContexts.EnemySpawners.Domain.Data
{
    public class RuntimeEnemySpawnerConfig
    {
        // public RuntimeEnemySpawnerConfig(EnemySpawnerConfig enemySpawnerConfig)
        // {
        //     StartEnemyAttackPower = enemySpawnerConfig.StartEnemyAttackPower;
        //     AddedEnemyAttackPower = enemySpawnerConfig.AddedEnemyAttackPower;
        //     StartEnemyHealth = enemySpawnerConfig.StartEnemyHealth;
        //     AddedEnemyHealth = enemySpawnerConfig.AddedEnemyHealth;
        //     StartBossAttackPower = enemySpawnerConfig.StartBossAttackPower;
        //     AddedBossAttackPower = enemySpawnerConfig.AddedBossAttackPower;
        //     StartBossMassAttackPower = enemySpawnerConfig.StartBossMassAttackPower;
        //     AddedBossMassAttackPower = enemySpawnerConfig.AddedBossMassAttackPower;
        //     StartBossHealth = enemySpawnerConfig.StartBossHealth;
        //     AddedBossHealth = enemySpawnerConfig.AddedBossHealth;
        //     StartKamikazeMassAttackPower = enemySpawnerConfig.StartKamikazeMassAttackPower;
        //     AddedKamikazeMassAttackPower = enemySpawnerConfig.AddedKamikazeMassAttackPower;
        //     StartKamikazeAttackPower = enemySpawnerConfig.StartKamikazeAttackPower;
        //     AddedKamikazeAttackPower = enemySpawnerConfig.AddedKamikazeAttackPower;
        //     StartKamikazeHealth = enemySpawnerConfig.StartKamikazeHealth;
        //     AddedKamikazeHealth = enemySpawnerConfig.AddedKamikazeHealth;
        //     
        //     foreach (EnemySpawnerWave wave in enemySpawnerConfig.Waves)
        //         _waves.Add(new RuntimeEnemySpawnerWave()
        //         {
        //             WaveId = wave.WaveId,
        //             SpawnDelay = wave.SpawnDelay,
        //             EnemyCount = wave.EnemyCount,
        //             BossesCount = wave.BossesCount,
        //             KamikazeEnemyCount = wave.KamikazeEnemyCount,
        //             MoneyPerResilenceCharacters = wave.MoneyPerResilenceCharacters,
        //         });
        // }

        public IReadOnlyList<RuntimeEnemySpawnerWave> Waves { get; set; }

        public int StartEnemyAttackPower { get; set; }
        public int AddedEnemyAttackPower { get; set; }
        public int StartEnemyHealth { get; set; }
        public int AddedEnemyHealth { get; set; }
        
        //Boss
        public int StartBossAttackPower { get; set; }
        public int AddedBossAttackPower { get; set; }
        public int StartBossMassAttackPower { get; set; }
        public int AddedBossMassAttackPower { get; set; }
        public int StartBossHealth { get; set; }
        public int AddedBossHealth { get; set; }
        
        //Kamikaze
        public int StartKamikazeMassAttackPower { get; set; }
        public int AddedKamikazeMassAttackPower { get; set; }
        public int StartKamikazeAttackPower { get; set; }
        public int AddedKamikazeAttackPower { get; set; }
        public int StartKamikazeHealth { get; set; }
        public int AddedKamikazeHealth { get; set; }
    }
}