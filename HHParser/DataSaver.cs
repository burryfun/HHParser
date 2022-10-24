using HHParser.Types;
using System.Net;
using System.Text.RegularExpressions;

// (&[\w\/]+;)|(<[\w\/]+>)|(<[\w\/]+ \/>) ПАТТЕРН ДЛЯ УДАЛЕНИЯ HTML ТЕГОВ

namespace HHParser
{
    public class DataSaver
    {
        public static readonly string APP_PATH;

        public static readonly string DATA_PATH; 

        static DataSaver()
        {
            APP_PATH = Directory.GetCurrentDirectory();
            DATA_PATH = APP_PATH + "/data";
        }

        // Создает папку для загружаемых данных
        // Удаляет HTML-атрибуты в описании вакансии
        // Вызывает функции сохранения данных о компании и сохранения логотипа
        public static async Task Save(RequiredEmployerData requiredEmployerData)
        {
            if (!Directory.Exists(DATA_PATH))
            {
                Directory.CreateDirectory(DATA_PATH);
            }

            Regex regex = new Regex(@"(&[\w\/]+;)|(<[\w\/]+>)|(<[\w\/]+ \/>)");
            var handledDescription = regex.Replace(requiredEmployerData.Description, "");

            await SaveEmployerDataAsync(requiredEmployerData.Name, handledDescription, requiredEmployerData.Vacancies);

            if (requiredEmployerData.LogoUrl != null)
            {
                await SaveImage(requiredEmployerData.LogoUrl, requiredEmployerData.Name);
            }
        }

        // Сохраняет логотип компании
        private static async Task SaveImage(string imageUrl, string fileName)
        {
            using (WebClient client = new WebClient())
            {
                var dirPath = Path.Combine(DATA_PATH, fileName);
                var imagePath = Path.Combine(dirPath, $"{fileName}.jpg");

                if (!Directory.Exists(dirPath))
                {
                    Directory.CreateDirectory(dirPath);
                }

                if (!File.Exists(imagePath))
                {
                    await client.DownloadFileTaskAsync(imageUrl, imagePath);
                }
                else
                {
                    //Console.ForegroundColor = ConsoleColor.Red;
                    //Console.WriteLine($"File {imagePath} already exists");
                    //Console.ResetColor();
                }
            }

        }

        // Сохраняет в текстовый файл название компании, описание и список вакансий
        private static async Task SaveEmployerDataAsync(string employerName, string employerDescription, List<Vacancy> employerVacancies)
        {
            var dirPath = Path.Combine(DATA_PATH, employerName);
            var filePath = Path.Combine(dirPath, $"{employerName}.txt");

            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }

            using (StreamWriter writer = new StreamWriter(filePath, false))
            {
                var text = $"Название компании: {employerName}\n\nОписание:\n{employerDescription}\n\n";

                if (employerVacancies.Count != 0)
                {
                    text += "Вакансии:";
                    int i = 1;
                    foreach (var vacancy in employerVacancies)
                    {
                        text += $"\n{i}. {vacancy.Name}\n\t";
                        i++;
                        
                        if (vacancy.Salary == null)
                        {
                            text += "з/п не указана";
                            continue;
                        }

                        if (vacancy.Salary.To == null)
                        {
                            text += $"от {vacancy.Salary.From} {vacancy.Salary.Currency}";
                        }
                        else if (vacancy.Salary.From == null)
                        {
                            text += $" до {vacancy.Salary.To} {vacancy.Salary.Currency}";
                        }
                        else
                        {
                            text += "з/п не указана";
                        }
                    }
                }
                else
                {
                    text += "Вакансий нет";
                }
                await writer.WriteLineAsync(text);
            }
        }
    }

    public struct RequiredEmployerData
    {
        public string Name;
        public string Description;
        public string? LogoUrl;
        public List<Vacancy> Vacancies;
    }
}
