using Newtonsoft.Json;
using Sources.DomainInterfaces.Models.Data;

namespace Sources.Domain.Models.Data
{
    public class ScoreCounterDto : IDto
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        
        [JsonProperty("totalScore")]
        public int TotalScore { get; set; }
    }
}