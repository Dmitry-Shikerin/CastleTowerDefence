using Newtonsoft.Json;

namespace Sources.BoundedContexts.EnemySpawners.Domain.Data
{
    public class RuntimeEnemySpawnerWave
    {
        public int WaveId { get; set; }
        
        //Enemies
        public int SpawnDelay { get; set; }
        public int EnemyCount { get; set; }
        
        //Bosses
        public int BossesCount { get; set; }
        
        //Kamikaze
        public int KamikazeEnemyCount { get; set; }
        
        //Money
        public int MoneyPerResurrectCharacters { get; set; }
        
        //Common
        [JsonIgnore]
        public int SumEnemies => EnemyCount + BossesCount + KamikazeEnemyCount;
    }
}