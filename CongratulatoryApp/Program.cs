using CongratulatoryApp;
using System;
using System.Text;
using System.Globalization;

class Program
{
    private static RecordManager _recordManager = new RecordManager();
    static void Main(string[] args)
    {
        using (var db = new AppDbContext())
        {

            Console.OutputEncoding = Encoding.UTF8;

            OutputUpcomingBirthdays(db);

            while (true)
            {
                Console.Clear();
                ChangeColorMenu("==================== Список ДР =====================");
                ChangeColorMenu("|                                                  |");
                ChangeColorMenu("|    1. Добавить запись                            |");
                ChangeColorMenu("|    2. Просмотр списка дней рождения              |");
                ChangeColorMenu("|    3. Просмотр списка ближайших дней рождения    |");
                ChangeColorMenu("|    4. Удалить запись                             |");
                ChangeColorMenu("|    5. Редактирование записей                     |");
                ChangeColorMenu("|    6. Завершение работы                          |");
                ChangeColorMenu("|                                                  |");
                ChangeColorMenu("====================================================");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(" Выберите опцию:");
                Console.Write(" ->");
                var choice = Console.ReadLine();
                Console.ResetColor();

                switch (choice)
                {
                    case "1":
                        InputNewRecord();
                        break;
                    case "2":
                        OutputRecords(db);
                        break;
                    case "3":
                        OutputUpcomingBirthdays(db);
                        break;
                    case "4":
                        DeleteRecord(db);
                        break;
                    case "5":
                        EditRecords(db);
                        break;
                    case "6":
                        return;
                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(" Неверная опция, попробуйте снова.");
                        Console.ResetColor();
                        break;
                }
                Console.WriteLine(" Нажмите любую клавишу для продолжения ");
                Console.ReadKey();
            }
        }
    }
    private static void InputNewRecord()
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine(" Введите имя:");
        string _name = Console.ReadLine();
        Console.WriteLine(" Введите дату дня рождения (в формате ДД.ММ.ГГГГ):");
        string _birthday = Console.ReadLine();
        if (DateTime.TryParseExact(_birthday, "dd.MM.yyyy", null, DateTimeStyles.None, out DateTime birthday))
        {
            _recordManager.Input(_name, birthday);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(" Запись добавлена!!! ");
        }
        else
        {
            Console.WriteLine(" Некорректный формат даты.");
        }
        Console.ResetColor();
    }
    private static void OutputRecords(AppDbContext db)
    {
        var birthdays = db.birthdays;
        SortRecords(birthdays);
    }
    private static void OutputUpcomingBirthdays(AppDbContext db)
    {
        var today = DateTime.SpecifyKind(DateTime.Today, DateTimeKind.Utc);
        var upcoming = db.birthdays
            .Where(b => b.birthday.Day >= today.Day && b.birthday.Month == today.Month && b.birthday.Year == today.Year
            || b.birthday.Year == today.Year && b.birthday.Month == today.AddMonths(1).Month);
        SortRecords(upcoming);
    }
    private static void SortRecords(IQueryable<Record> birthdays)
    {
        var choice = "1";
        while (true)
        {
            Console.Clear();

            switch (choice)
            {
                case "1":
                    Console.WriteLine("\n Список записей:");
                    _recordManager.Output(birthdays.ToList());
                    break;
                case "2":
                    Console.WriteLine("\n Список записей:");
                    _recordManager.Output(birthdays.OrderBy(b => b.id).ToList());
                    break;
                case "3":
                    Console.WriteLine("\n Список записей:");
                    _recordManager.Output(birthdays.OrderBy(b => b.birthday).ToList());
                    break;
                case "4":
                    return;
                default:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\n Неверная опция, попробуйте снова.");
                    Console.ResetColor();
                    break;
            }

            ChangeColorMenu("\n================ Список ДР ===============");
            ChangeColorMenu("|                                        |");
            ChangeColorMenu("|    1. Вывести без сортировки           |");
            ChangeColorMenu("|    2. Вывести с сортировкой по ID      |");
            ChangeColorMenu("|    3. Вывести с сортировкой по дате    |");
            ChangeColorMenu("|    4. Главное меню                     |");
            ChangeColorMenu("|                                        |");
            ChangeColorMenu("==========================================");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(" Выберите опцию:");
            Console.Write(" ->");
            choice = Console.ReadLine();
            Console.ResetColor();

            Console.WriteLine(" Нажмите любую клавишу для продолжения ");
            Console.ReadKey();
        }
    }
    private static void DeleteRecord(AppDbContext db)
    {
        Console.Write("Введите ID записи для удаления: ");
        if (int.TryParse(Console.ReadLine(), out int id))
        {
            _recordManager.Delete(db, id);
        }
        else
        {
            Console.WriteLine("Некорректный ID.");
        }
    }
    private static void EditRecords(AppDbContext db)
    {
        Console.Write("Введите ID записи для редактирования: ");
        if (int.TryParse(Console.ReadLine(), out int id))
        {
            _recordManager.Edit(db, id);
        }
        else
        {
            Console.WriteLine("Некорректный ID.");
        }
    }
    private static void ChangeColorMenu(string message)
    {
        foreach (char c in message)
        {
            if (c == '|' || c == '=')
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.Write(c);
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write(c);
                Console.ResetColor();
            }
        }
        Console.WriteLine();
    }
}