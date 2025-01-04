using System.Collections.ObjectModel;

namespace PicSender.Models;

public class PictureGroup
{
    public string Title { get; set; }
    public ObservableCollection<SinglePicture> Pictures { get; } = [];
    
    public int PictureCount => Pictures.Count;
    
}