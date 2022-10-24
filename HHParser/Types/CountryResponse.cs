using Newtonsoft.Json;

namespace HHParser.Types
{
    public class CountryResponse
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("name")]
        public string? Name { get; set; }
    }
}
