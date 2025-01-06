namespace PicSender.Models;

public class PictureGroup
{
    public string Title { get; set; }
    public List<SinglePicture> Pictures { get; } = [];
    
    public int PictureCount => Pictures.Count;
    
}