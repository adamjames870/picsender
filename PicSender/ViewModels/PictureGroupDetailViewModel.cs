using System.Collections.ObjectModel;
using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PicSender.Models;
using PicSender.Services;

namespace PicSender.ViewModels;

[QueryProperty(nameof(PictureGroup), "PictureGroup")]
public partial class PictureGroupDetailViewModel : BaseViewModel
{
    
    private PicDatabase database;
    
    [ObservableProperty]
    private PictureGroup _pictureGroup;

    [ObservableProperty]
    private ObservableCollection<SinglePicture> _pictures;
    
    private IMediaPicker _mediaPicker;

    public PictureGroupDetailViewModel(IMediaPicker mediaPicker, PicDatabase db)
    {
        database = db;
        _mediaPicker = mediaPicker;
    }

    public async Task LoadPicturesAsync()
    {
        try
        {
            var pics = await database.GetPicturesAsync(PictureGroup.Id) ?? [];
            Pictures = new ObservableCollection<SinglePicture>(pics);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error: {ex.Message}");
            await Shell.Current.DisplayAlert(nameof(LoadPicturesAsync), $"Exception: {ex.Message}", "OK");
        }
    }
    
    [RelayCommand]
    async Task PickPictureAsync()
    {
        
        try
        {
            var picture = await _mediaPicker.PickPhotoAsync();
            if (picture is null) return;
            var name = await App.Current.MainPage.DisplayPromptAsync("Name", "Enter a name for the picture");
            Pictures.Add(new SinglePicture { Name = name, FullPath = picture.FullPath });
            await database.AddPictureAsync(new SinglePicture { Name = name, FullPath = picture.FullPath, PictureGroupId = PictureGroup.Id });
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error: {ex.Message}");
            await Shell.Current.DisplayAlert("Error", $"Exception: {ex.Message}", "OK");
        }
        finally
        {
            
        }

    }
    
    [RelayCommand]
    async Task TakePictureAsync()
    {

        try
        {
            if (!_mediaPicker.IsCaptureSupported)
            {
                await Shell.Current.DisplayAlert("Error", "Capture is not supported on this device", "OK");
                return;
            }
            var picture = await _mediaPicker.CapturePhotoAsync();
            if (picture is null) return;
            var name = await App.Current.MainPage.DisplayPromptAsync("Name", "Enter a name for the picture");
            Pictures.Add(new SinglePicture { Name = name, FullPath = picture.FullPath });
            await database.AddPictureAsync(new SinglePicture { Name = name, FullPath = picture.FullPath, PictureGroupId = PictureGroup.Id }); 
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error: {ex.Message}");
            await Shell.Current.DisplayAlert("Error", $"Exception: {ex.Message}", "OK");
        }
        finally
        {
            
        }

    }
    
}