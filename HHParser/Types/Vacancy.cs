
using Newtonsoft.Json;

namespace HHParser.Types
{
    public class Vacancy
    {
        [JsonProperty("alternate_url")]
        public string Url { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("salary")]
        public Salary? Salary { get; set; }
    }
}
