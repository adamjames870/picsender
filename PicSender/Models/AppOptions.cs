using SQLite;

namespace PicSender.Models;

public class AppOptions
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string EmailAddress { get; set; }
}