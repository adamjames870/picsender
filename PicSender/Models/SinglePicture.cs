using SQLite;
using SQLiteNetExtensions.Attributes;

namespace PicSender.Models;

public class SinglePicture
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string Name { get; set; }
    public string? FullPath { get; set; }
    [ManyToOne]
    public PictureGroup PictureGroup { get; set; }
}