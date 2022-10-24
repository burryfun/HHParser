using HHParser;

var country = "Казахстан";
var city = "Астана";

// Текст для поиска. Переданное значение ищется в названии компании
// Если пустая строка, то ищет все компании
var textSearch = "";

var hhParser = await HeadHunterParser.CreateAsync(country, city);

var employersData = await hhParser.GetEmployersDataAsync(textSearch);
var totalEmployers = employersData.Count;

Console.WriteLine($"Кол-во найденных компаний: {totalEmployers}\n");
Console.WriteLine($"Нажмите Enter, чтобы сохранить данные этих компаний\n");

var input = Console.ReadKey();
if (input.Key != ConsoleKey.Enter)
{
    Environment.Exit(0);
}

Console.Write("Загрузка данных... ");
var progress = new ProgressBar();

for (int page = 0; page < employersData.TotalPages; page++)
{
    employersData = await hhParser.GetEmployersDataAsync(textSearch, page);
    foreach (var employer in employersData.Employers)
    {
        RequiredEmployerData requiredEmployerData = new RequiredEmployerData();

        var task = new Task(async () =>
        {
            requiredEmployerData.Name = employer.Name.Replace(@"/", "").Trim();
            requiredEmployerData.Description = await hhParser.GetEmployerDescriptionByIdAsync(employer.Id);
            requiredEmployerData.Vacancies = (await hhParser.GetVacanciesDataAsync(employer.Id)).Vacancies;

            if (employer.logoUrls != null)
            {
                requiredEmployerData.LogoUrl = employer.logoUrls.Size240;
            }

            await DataSaver.Save(requiredEmployerData);
        });

        task.Start();
        Thread.Sleep(100);
    }
    progress.Report((double) page / employersData.TotalPages);
}

Task.WaitAll();
progress.Dispose();
Console.WriteLine("Завершена.\n");

Console.WriteLine("Нажмите Enter для выхода.\n");

input = Console.ReadKey();
if (input.Key == ConsoleKey.Enter)
{
    Environment.Exit(0);
}
