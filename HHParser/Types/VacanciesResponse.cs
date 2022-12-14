
using Newtonsoft.Json;

namespace HHParser.Types
{
    public class VacanciesResponse
    {
        [JsonProperty("found")]
        public int Count { get; set; }
        [JsonProperty("items")]
        public List<Vacancy> Vacancies { get; set; }
        [JsonProperty("page")]
        public int CurrentPage { get; set; }
        [JsonProperty("pages")]
        public int TotalPages { get; set; }
        [JsonProperty("per_page")]
        public int PerPage { get; set; }
    }
}
