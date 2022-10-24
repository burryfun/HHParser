using Newtonsoft.Json;

namespace HHParser.Types
{
    public class Employer
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("url")]
        public string Url { get; set; }
        [JsonProperty("alternate_url")]
        public string AlternateUrl { get; set; }
        [JsonProperty("open_vacancies")]
        public int TotalVacancies { get; set; }
        [JsonProperty("vacancies_url")]
        public string VacanciesUrl { get; set; }
        [JsonProperty("logo_urls")]
        public LogoUrls? logoUrls { get; set; }
    }
}
