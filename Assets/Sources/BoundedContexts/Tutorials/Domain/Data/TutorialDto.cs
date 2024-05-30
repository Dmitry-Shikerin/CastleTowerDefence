using Newtonsoft.Json;
using Sources.Frameworks.Domain.Interfaces.Data;

namespace Sources.Domain.Models.Data
{
    public class TutorialDto : IDto
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("hasCompleted")]
        public bool HasCompleted { get; set; }
    }
}