using Newtonsoft.Json;
using Sources.DomainInterfaces.Models.Data;

namespace Sources.Domain.Models.Data
{
    public class VolumeDto : IDto
    {
        [JsonProperty("musicValue")]
        public float MusicValue { get; set; }
        
        [JsonProperty("miniGunVolumeValue")]
        public float MiniGunVolumeValue { get; set; }
        
        [JsonProperty("id")]
        public string Id { get; set; }
    }
}