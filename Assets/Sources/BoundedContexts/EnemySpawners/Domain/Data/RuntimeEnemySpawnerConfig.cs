using System.Collections.Generic;

namespace Sources.BoundedContexts.EnemySpawners.Domain.Data
{
    public class RuntimeEnemySpawnerConfig
    {
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