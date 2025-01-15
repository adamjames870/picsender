using CommunityToolkit.Mvvm.ComponentModel;
using PicSender.Models;
using PicSender.Services;

namespace PicSender.ViewModels;

public class PictureGroupItemModel(PictureGroup pictureGroup)
{
    public int PictureGroupId => pictureGroup.Id;
    public new string Title => pictureGroup.Title;
    public string PictureCount => pictureGroup.PictureIds.Count.ToString();
}