using Newtonsoft.Json;
using Sources.DomainInterfaces.Models.Data;

namespace Sources.Domain.Models.Data
{
    public class EnemySpawnerDto : IDto
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        
        [JsonProperty("bossesInLevel")]
        public int BossesInLevel { get; set; }
        
        [JsonProperty("enemyInWave")]
        public int[] EnemyInWave { get; set; }
        
        [JsonProperty("spawnDelays")]
        public int[] SpawnDelays { get; set; }
        
        [JsonProperty("spawnedEnemies")]
        public int SpawnedEnemies { get; set; }
        
        [JsonProperty("spawnedBosses")]
        public int SpawnedBosses { get; set; }
        
        [JsonProperty("currentWave")] 
        public int CurrentWave { get; set; }

    }
}