public class Record
{
    public int id { get; set; }
    public string name { get; set; }
    public DateTime birthday { get; set; }
    public override string ToString()
    {
        return $"| ID: {id} | Дата: {birthday.Day}/{birthday.Month}/{birthday.Year} | Имя: {name} |";
    }
}
