using SQLite;
using SQLiteNetExtensions.Attributes;

namespace PicSender.Models;

public class PictureGroup
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string Title { get; set; }
    [OneToMany(CascadeOperations = CascadeOperation.All)]
    public List<SinglePicture> Pictures { get; } = [];
    
    public int PictureCount => Pictures.Count;
    
}