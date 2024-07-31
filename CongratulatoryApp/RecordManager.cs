using System;
using System.Globalization;

namespace CongratulatoryApp
{
    public class RecordManager
    {
        public void Output(List<Record> birthdays)
        {
            foreach (var _birthday in birthdays)
            {
                Console.WriteLine(_birthday);
            }
        }
        public void Input(string _name, DateTime _birthday)
        {
            var birthdayUtc = DateTime.SpecifyKind(_birthday, DateTimeKind.Utc);
            using (var db = new AppDbContext())
            {
                var newRecord = new Record { name = _name, birthday = birthdayUtc };
                db.birthdays.Add(newRecord);
                db.SaveChanges();
            }
        }
        public void Delete(AppDbContext db, int id)
        {
            var delBirthday = db.birthdays.Find(id);
            if (delBirthday != null)
            {
                db.birthdays.Remove(delBirthday);
                db.SaveChanges();
                Console.WriteLine("Запись удалена.");
            }
            else
            {
                Console.WriteLine("Запись не найдена.");
            }
        }
        public void Edit(AppDbContext db, int id)
        {
            var editRecord = db.birthdays.Find(id);
            if (editRecord != null)
            {
                Console.Write("Введите новое имя (оставьте пустым для сохранения текущего): ");
                var editName = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(editName))
                {
                    editRecord.name = editName;
                }

                Console.Write("Введите новую дату рождения (ДД.ММ.ГГГГ, оставьте пустым для сохранения текущей): ");
                var editBirthday = Console.ReadLine();
                if (DateTime.TryParseExact(editBirthday, "dd.MM.yyyy", null, DateTimeStyles.None, out DateTime birthday))
                {
                    var editBirthdayUtc = DateTime.SpecifyKind(birthday, DateTimeKind.Utc);
                    editRecord.birthday = editBirthdayUtc;
                }

                db.SaveChanges();
                Console.WriteLine("Запись обновлена.");
            }
            else
            {
                Console.WriteLine("Запись не найдена.");
            }
        }
    }
}
