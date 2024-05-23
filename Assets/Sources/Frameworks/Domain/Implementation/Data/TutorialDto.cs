using Newtonsoft.Json;
using Sources.DomainInterfaces.Models.Data;

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