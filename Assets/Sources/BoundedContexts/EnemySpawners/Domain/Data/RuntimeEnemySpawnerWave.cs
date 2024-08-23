using Newtonsoft.Json;
using Sources.BoundedContexts.EnemySpawners.Domain.Configs;

namespace Sources.BoundedContexts.EnemySpawners.Domain.Data
{
    public class RuntimeEnemySpawnerWave
    {
        // public RuntimeEnemySpawnerWave(EnemySpawnerWave wave)
        // {
        //     WaveId = wave.WaveId;
        //     SpawnDelay = wave.SpawnDelay;
        //     EnemyCount = wave.EnemyCount;
        //     BossesCount = wave.BossesCount;
        //     KamikazeEnemyCount = wave.KamikazeEnemyCount;
        //     MoneyPerResilenceCharacters = wave.MoneyPerResilenceCharacters;
        // }
        
        public int WaveId { get; set; }
        
        //Enemys
        public int SpawnDelay { get; set; }
        public int EnemyCount { get; set; }
        
        //Bosses
        public int BossesCount { get; set; }
        
        //Kamikaze
        public int KamikazeEnemyCount { get; set; }
        
        //Money
        public int MoneyPerResilenceCharacters { get; set; }
        
        //Common
        [JsonIgnore]
        public int SumEnemies => EnemyCount + BossesCount + KamikazeEnemyCount;
    }
}