using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using PicSender.Models;

namespace PicSender.ViewModels;

public partial class MainPageViewModel : BaseViewModel
{
    public ObservableCollection<PicToSend> Pics { get; } = [];
    
    public MainPageViewModel()
    {
        Pics.Add(new PicToSend { Name = "Pic 1" });
        Pics.Add(new PicToSend { Name = "Pic 2" });
        Pics.Add(new PicToSend { Name = "Pic 3" });
    }

    [RelayCommand]
    async Task AddPictureAsync()
    {
        
        var picture = await MediaPicker.CapturePhotoAsync();
        if (picture is null) return;
        Pics.Add(new PicToSend { Name = picture.FullPath });
        
    }
    
    [RelayCommand]
    async Task TakePictureAsync()
    {

        var picture = await MediaPicker.CapturePhotoAsync();
        if (picture is null) return;
        Pics.Add(new PicToSend { Name = picture.FullPath });

    }
    
}