using CommunityToolkit.Mvvm.ComponentModel;
using PicSender.Models;
using PicSender.Services;

namespace PicSender.ViewModels;

public class PictureGroupItemModel(PictureGroup pictureGroup)
{
    public int PictureGroupId => pictureGroup.Id;
    public new string Title => pictureGroup.Title;
    public int PictureCount => pictureGroup.PictureIds.Count;
}