using PicSender.Models;

namespace PicSender.ViewModels.ItemModels;

public class PictureGroupItemModel(PictureGroup pictureGroup) : BaseViewModel
{
    public int PictureGroupId => pictureGroup.Id;
    public new string Title => pictureGroup.Title;
    public string PictureCount => pictureGroup.PictureCount.ToString();
    public string? ThumbnailPath => pictureGroup.ThumbnailPath ?? "noimage100.png";    
    
    public PictureGroup ChangeTitle(string newTitle)
    {
        pictureGroup.Title = newTitle;
        OnPropertyChanged(nameof(Title));
        return pictureGroup;
    }

}