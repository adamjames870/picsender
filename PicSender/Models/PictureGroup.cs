using SQLite;
using SQLiteNetExtensions.Attributes;

namespace PicSender.Models;

public class PictureGroup
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string Title { get; set; }
    
    public string? ThumbnailPath { get; set; }

    public int PictureCount { get; set; } = 0;

}