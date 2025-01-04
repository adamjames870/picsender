using System.Collections.ObjectModel;
using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PicSender.Models;
using PicSender.Views;

namespace PicSender.ViewModels;

[QueryProperty(nameof(PictureGroup), "PictureGroup")]
public partial class PictureGroupDetailViewModel : BaseViewModel
{
    [ObservableProperty]
    private PictureGroup _pictureGroup;
    
    private IMediaPicker _mediaPicker;

    public PictureGroupDetailViewModel(IMediaPicker mediaPicker)
    {
        _mediaPicker = mediaPicker;
    }

    [RelayCommand]
    async Task PickPictureAsync()
    {

        try
        {
            var picture = await _mediaPicker.PickPhotoAsync();
            if (picture is null) return;
            PictureGroup.Pictures.Add(new SinglePicture { Name = picture.FullPath });
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
            PictureGroup.Pictures.Add(new SinglePicture { Name = picture.FullPath }); 
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