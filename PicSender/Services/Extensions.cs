using System.Collections.ObjectModel;
using PicSender.Models;
using PicSender.ViewModels;

namespace PicSender.Services;

public static class Extensions
{
    public static ObservableCollection<T> ToObservableCollection<T>(this List<T> enumerable)
    {
        return new ObservableCollection<T>(enumerable);
    }

    public static PictureGroupItemModel ToPictureGroupItemModel(this PictureGroup pictureGroup)
    {
        return new PictureGroupItemModel(pictureGroup);
    }
    
}