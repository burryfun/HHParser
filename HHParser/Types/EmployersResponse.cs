using Newtonsoft.Json;

namespace HHParser.Types
{
    public class EmployersResponse
    {
        [JsonProperty("found")]
        public int Count { get; set; }
        [JsonProperty("items")]
        public List<Employer> Employers { get; set; }
        [JsonProperty("page")]
        public int CurrentPage { get; set; }
        [JsonProperty("pages")]
        public int TotalPages { get; set; }
        [JsonProperty("per_page")]
        public int PerPage { get; set; }
    }
}
