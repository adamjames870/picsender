using SQLite;
using SQLiteNetExtensions.Attributes;

namespace PicSender.Models;

public class PictureGroup
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string Title { get; set; }
    public List<int> PictureIds { get; } = [];
    
    public int PictureCount => PictureIds.Count;
    
}