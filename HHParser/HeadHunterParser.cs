using HHParser.Types;
using Newtonsoft.Json;

namespace HHParser
{
    public class HeadHunterParser
    {
        private HttpClient _httpClient;
        private const string _url = "http://api.hh.ru";

        private int _countryId;
        private int _cityId;

        private HeadHunterParser()
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "HHParser");
        }

        public static async Task<HeadHunterParser> CreateAsync(string countryName, string cityName)
        {
            HeadHunterParser parser = new HeadHunterParser();

            var countryId = await parser.FindCountryIdByCountryNameAsync(countryName);
            var cityId = await parser.FindCityIdByCityNameAsync(countryId, cityName);

            parser._countryId = countryId;
            parser._cityId = cityId;

            return parser;
        }

        // Получение данных о работадателях, включая часть списка работадателей по региону _cityId
        // Опиональные параметры:
        // text -   Текст для поиска. Переданное значение ищется в названии компании
        //          Если пустая строка, то ищет все компании
        // openVacancies - по умолчанию ищет работадателей, у который есть открытые вакансии
        public async Task<EmployersResponse> GetEmployersDataAsync(string text = "", int page = 0, bool openVacancies = true)
        {
            var response = await _httpClient.GetAsync(
                $"{_url}/employers?text={text}&area={_cityId}&only_with_vacancies={openVacancies}&page={page}"
                );

            if (response.IsSuccessStatusCode == false)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"ERROR: {error}");
            }

            var responseJson = await response.Content.ReadAsStringAsync();
            var employersDataResponse = JsonConvert.DeserializeObject<EmployersResponse>(responseJson);

            return employersDataResponse;

        }

        // Получение данных о вакансиях, включая часть списка вакансий по id работадателя.
        public async Task<VacanciesResponse> GetVacanciesDataAsync(int employerId)
        {
            var response = await _httpClient.GetAsync($"{_url}/vacancies?employer_id={employerId}&per_page=100");

            if (response.IsSuccessStatusCode == false)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"ERROR: {error}");
            }

            var responseJson = await response.Content.ReadAsStringAsync();
            var vacanciesDataResponse = JsonConvert.DeserializeObject<VacanciesResponse>(responseJson);

            return vacanciesDataResponse;
        }

        // Получение описания вакансии по id работадателя.
        public async Task<string> GetEmployerDescriptionByIdAsync(int employerId)
        {
            var response = await _httpClient.GetAsync($"{_url}/employers/{employerId}");

            if (response.IsSuccessStatusCode == false)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"ERROR: {error}");
            }

            var responseJson = await response.Content.ReadAsStringAsync();
            var employerDetails = JsonConvert.DeserializeObject<EmployerDetailsResponse>(responseJson);

            return employerDetails.Description ?? "Нет описания";
        }



        // Поиск id страны по названию.
        private async Task<int> FindCountryIdByCountryNameAsync(string countryName)
        {
            var response = await _httpClient.GetAsync($"{_url}/areas/countries");

            if (response.IsSuccessStatusCode == false)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"ERROR: {error}");
            }

            var responseJson = await response.Content.ReadAsStringAsync();
            IEnumerable<CountryResponse>? countries = JsonConvert.DeserializeObject<IEnumerable<CountryResponse>>(responseJson);

            var country = countries.FirstOrDefault(u => u.Name == countryName);

            if (country == null)
            {
                throw new Exception($"ERROR: Страна {countryName} не найдена");
            }

            return country.Id;
        }

        // Поиск id города по названию и id страны, к которой относится искомый город.
        private async Task<int> FindCityIdByCityNameAsync(int countryId, string cityName)
        {
            var response = await _httpClient.GetAsync($"{_url}/areas/{countryId}");

            if (response.IsSuccessStatusCode == false)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"ERROR: {error}");
            }

            var responseJson = await response.Content.ReadAsStringAsync();
            CityResponse? area = JsonConvert.DeserializeObject<CityResponse>(responseJson);

            var city = area.Areas.FirstOrDefault(u => u.Name == cityName);

            if (city == null)
            {
                throw new Exception($"ERROR: Город {cityName} не найден");
            }

            return city.Id;
        }
    }
}
