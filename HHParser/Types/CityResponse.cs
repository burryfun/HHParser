using Newtonsoft.Json;

namespace HHParser.Types
{
    public class CityResponse
    {
        [JsonProperty("areas")]
        public CityResponse[] Areas { get; set; }
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("name")]
        public string? Name { get; set; }
        [JsonProperty("parent_id")]
        public int? ParentId { get; set; }
    }
}
