using System.Collections.ObjectModel;
using System.Diagnostics;
using CommunityToolkit.Mvvm.Input;
using PicSender.Models;

namespace PicSender.ViewModels;

public partial class MainPageViewModel : BaseViewModel
{
    public ObservableCollection<PicToSend> Pics { get; } = [];

    private IMediaPicker _mediaPicker;
    
    public MainPageViewModel(IMediaPicker mediaPicker)
    {
        _mediaPicker = mediaPicker;
        Pics.Add(new PicToSend { Name = "Pic 1" });
        Pics.Add(new PicToSend { Name = "Pic 2" });
        Pics.Add(new PicToSend { Name = "Pic 3" });
    }

    [RelayCommand]
    async Task PickPictureAsync()
    {

        try
        {
            var picture = await _mediaPicker.PickPhotoAsync();
            if (picture is null) return;
            Pics.Add(new PicToSend { Name = picture.FullPath });
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
           Pics.Add(new PicToSend { Name = picture.FullPath }); 
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