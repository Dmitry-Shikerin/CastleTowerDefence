using Newtonsoft.Json;

namespace Sources.Domain.Models.Data
{
    public class MoneyPerUpgradeDto
    {
        [JsonProperty("moneyPerUpgradeCharisma")]
        public int MoneyPerUpgrade { get; set; }
    }
}