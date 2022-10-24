using Newtonsoft.Json;

namespace HHParser.Types
{
    public class Salary
    {
        [JsonProperty("currency")]
        public string? Currency { get; set; }
        [JsonProperty("from")]
        public int? From { get; set; }
        [JsonProperty("to")]
        public int? To { get; set; }
    }
}