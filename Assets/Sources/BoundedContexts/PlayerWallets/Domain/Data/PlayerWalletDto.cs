using Newtonsoft.Json;
using Sources.Frameworks.Domain.Interfaces.Data;

namespace Sources.Domain.Models.Data
{
    public class PlayerWalletDto : IDto
    {
        [JsonProperty("coins")]
        public int Coins { get; set; }
        
        [JsonProperty("id")]
        public string Id { get; set; }
    }
}