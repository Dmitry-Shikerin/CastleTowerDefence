using Newtonsoft.Json;
using Sources.Frameworks.Domain.Interfaces.Data;

namespace Sources.Frameworks.Domain.Implementation.Data
{
    public class GameDataDto : IDto
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        
        [JsonProperty("wasLaunched")]
        public bool WasLaunched { get; set; }
    }
}