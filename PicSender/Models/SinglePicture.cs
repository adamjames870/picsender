using SQLite;

namespace PicSender.Models;

public class SinglePicture
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string Name { get; set; }
    public string? FullPath { get; set; }
    public int PictureGroupId { get; set; }
}