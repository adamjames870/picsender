using PicSender.Models;

namespace PicSender.ViewModels.ItemModels;

public class PictureItemModel (SinglePicture singlePicture) : BaseViewModel
{
    public int PictureId => singlePicture.Id;
    public string Name => singlePicture.Name;
    public string? FullPath => singlePicture.FullPath;
    public SinglePicture ChangeName(string newName)
    {
        singlePicture.Name = newName;
        OnPropertyChanged(nameof(Name));
        return singlePicture;
    }
    
}