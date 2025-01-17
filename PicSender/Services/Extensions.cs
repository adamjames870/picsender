using PicSender.Models;
using PicSender.ViewModels.ItemModels;

namespace PicSender.Services;

public static class Extensions
{
    public static PictureGroupItemModel ToPictureGroupItemModel(this PictureGroup pictureGroup)
    {
        return new PictureGroupItemModel(pictureGroup);
    }
    
    

    public static PictureItemModel ToPictureItemModel(this SinglePicture singlePicture)
    {
        return new PictureItemModel(singlePicture);
    }
    
}