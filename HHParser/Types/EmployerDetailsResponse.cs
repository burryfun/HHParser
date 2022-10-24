using Newtonsoft.Json;

namespace HHParser.Types
{
    public class EmployerDetailsResponse
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("description")]
        public string? Description { get; set; }
    }
}
