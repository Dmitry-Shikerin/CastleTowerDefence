using Newtonsoft.Json;
using Sources.Frameworks.Domain.Interfaces.Data;

namespace Sources.BoundedContexts.Upgrades.Domain.Data
{
    public class UpgradeDto : IDto
    {
        [JsonProperty("currentLevel")]
        public int CurrentLevel { get; set; }
        
        [JsonProperty("maxLevel")]
        public int MaxLevel { get; set; }
        
        [JsonProperty("startAmount")]
        public float StartAmount { get; set; }
        
        [JsonProperty("addedAmount")]
        public float AddedAmount { get; set; }
        
        [JsonProperty("moneyPerUpgrades")]
        public int[] MoneyPerUpgrades { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }
    }
}