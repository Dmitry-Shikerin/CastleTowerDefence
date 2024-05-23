using Newtonsoft.Json;
using Sources.DomainInterfaces.Models.Data;

namespace Sources.Domain.Models.Data
{
    public class LevelDto : IDto
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        
        [JsonProperty("isCompleted")]
        public bool IsCompleted { get; set; }
    }
}