using Newtonsoft.Json;
using Sources.DomainInterfaces.Models.Data;

namespace Sources.Domain.Models.Data
{
    public class SavedLevelDto : IDto
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        
        [JsonProperty("isSaved")]
        public bool IsSaved { get; set; }
        
        [JsonProperty("savedLevelId")]
        public string SavedLevelId { get; set; }
    }
}