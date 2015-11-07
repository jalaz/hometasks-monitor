using System;
using System.Linq;
using HometasksMonitoringPanel.Configs;

namespace HometasksMonitoringPanel.Providers
{
    public class RepositoryProvider : IRepositoryProvider
    {
        private readonly GithubConfig _config;

        public RepositoryProvider(GithubConfig config)
        {
            _config = config;
        }

        public GitHubRepository[] GetAll()
        {
            return new Tuple<string, string>[]
            {
Tuple.Create("Сергей Пышкин", "Maklevskiy/SergHomeWork"),
Tuple.Create("Светлана Боберская", "Lanasvet/CourseKottans"),
Tuple.Create("Полина Сытник", "polinasytnyk/kittyworks"),
Tuple.Create("Володько Владислав", "vlad283/homework"),
Tuple.Create("Игорь Руденко", "Rudeka/KottansHomeWork"),
Tuple.Create("Андрій Сташко", "AndriiStashko/KottansHomeWork"),
Tuple.Create("Руслана Лагутина", "Zunderbird/kottans_homeworks"),
Tuple.Create("Инга Коротоножкина", "agni90/kottansHomeTasks"),
Tuple.Create("Андрій Ліницький", "Sbforecast/al_kottans"),
Tuple.Create("Ксения Гнатовская", "kgnatovska/acsharpharpoon"),
Tuple.Create("Александр Фёдоров", "olexandr17/kottans"),
Tuple.Create("Валерій Колеснік", "ValerCheck/sharp-kottans"),
Tuple.Create("Игорь Никора", "kolinlob/kottans"),
Tuple.Create("Евгений Поладич", "JackM10/HomeWork"),
Tuple.Create("Артём Борисенко", "s1dA/kottans"),
Tuple.Create("Денис Максимец", "maxymka94/kottansHW"),
Tuple.Create("Леньга Олег", "shamanishche/Kottans_Repo"),
Tuple.Create("Сергей Шевченко", "else-if/homeworks"),
Tuple.Create("Максим Хамровский", "maksim36ua/KottansHomework"),
Tuple.Create("Юлия Кривонос", "julia-kryvonos/CSharp-Homework"),
Tuple.Create("Ирина Кушнир", "iryna-kushnir/KottansHomeworks"),
Tuple.Create("Инна Завертана", "Innazvrtn/kottanshome"),
Tuple.Create("Ивченко Александр", "IvchenkoSanya/kottans"),
Tuple.Create("Полина Селюх", "PollyHusker/Kottans_hw2015"),
Tuple.Create("Денис Крюков", "krukovden/KottansHomework"),
Tuple.Create("Константин Зингер", "KZRKZR/KottansHomeWork"),
Tuple.Create("Дмитрий Трененков", "aspafenix/cs-course-hw"),
Tuple.Create("Сергей Воронов", "sergeyvoronov/kottans"),
Tuple.Create("Вадим Брацкий", "VadymBratskyi/Kottans-Homework"),
Tuple.Create("Степанюк Олександр", "ostepaniuk/CSharp")
            }
            .Select(n => new GitHubRepository
            {
                Name = n.Item1,
                RelativeUrl = n.Item2,
                AbsoluteUrl = _config.BaseSiteUrl + "/" + n.Item2
            })
            .ToArray();
        }
    }




    public class GitHubRepository
    {
        public string Name { get; set; }
        public string AbsoluteUrl { get; set; }
        public string RelativeUrl { get; set; }
    }

    public interface IRepositoryProvider
    {
        GitHubRepository[] GetAll();
    }
}