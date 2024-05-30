using Newtonsoft.Json;
using Sources.Frameworks.Domain.Interfaces.Data;

namespace Sources.Frameworks.Domain.Implementation.Data
{
    public class LevelDto : IDto
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        
        [JsonProperty("isCompleted")]
        public bool IsCompleted { get; set; }
    }
}