using System.Collections.ObjectModel;
using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PicSender.Models;
using PicSender.Services;

namespace PicSender.ViewModels;

[QueryProperty(nameof(PictureGroup), "PictureGroup")]
public partial class PictureGroupDetailViewModel(IMediaPicker mediaPicker, PicDatabase db) : BaseViewModel
{
    [ObservableProperty]
    private PictureGroup _pictureGroup = null!;

    [ObservableProperty]
    private ObservableCollection<SinglePicture> _pictures = null!;

    public async Task LoadPicturesAsync()
    {
        try
        {
            var pics = await db.GetPicturesAsync(PictureGroup.Id) ?? [];
            var picModels = pics.Select(p => p.ToPictureItemModel()).ToList();
            Pictures = new ObservableCollection<SinglePicture>(pics);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error: {ex.Message}");
            await Shell.Current.DisplayAlert(nameof(LoadPicturesAsync), $"Exception: {ex.Message}", "OK");
        }
    }
    
    [RelayCommand]
    private async Task PickPictureAsync()
    {
        
        try
        {
            var picture = await mediaPicker.PickPhotoAsync();
            if (picture is null) return;
            var name = await App.Current.MainPage.DisplayPromptAsync("Name", "Enter a name for the picture");
            Pictures.Add(new SinglePicture { Name = name, FullPath = picture.FullPath });
            await db.AddPictureAsync(new SinglePicture { Name = name, FullPath = picture.FullPath, PictureGroupId = PictureGroup.Id });
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
    private async Task TakePictureAsync()
    {

        try
        {
            if (!mediaPicker.IsCaptureSupported)
            {
                await Shell.Current.DisplayAlert("Error", "Capture is not supported on this device", "OK");
                return;
            }
            var picture = await mediaPicker.CapturePhotoAsync();
            if (picture is null) return;
            var name = await App.Current.MainPage.DisplayPromptAsync("Name", "Enter a name for the picture");
            Pictures.Add(new SinglePicture { Name = name, FullPath = picture.FullPath });
            await db.AddPictureAsync(new SinglePicture { Name = name, FullPath = picture.FullPath, PictureGroupId = PictureGroup.Id }); 
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
    async Task DeletePictureAsync(SinglePicture picture)
    {
        try
        {
            Pictures.Remove(picture);
            await db.DeletePictureAsync(picture);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error: {ex.Message}");
            await Shell.Current.DisplayAlert($"Error in {nameof(DeletePictureAsync)}", $"Exception: {ex.Message}, {ex.Source}", "OK");
        }
    }

    [RelayCommand]
    async Task RenamePictureAsync(SinglePicture picture)
    {
        try
        {
            var newName = await Shell.Current.DisplayPromptAsync("Name", "Enter the name for the picture", "OK", "Cancel", picture.Name);
            if (!string.IsNullOrEmpty(newName) && newName != picture.Name)
            {
                picture.Name = newName;
                OnPropertyChanged(nameof(picture.Name));
                await db.UpdatePictureAsync(picture);
            }
            
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error: {ex.Message}");
            await Shell.Current.DisplayAlert($"Error in {nameof(RenamePictureCommand)}", $"Exception: {ex.Message}, {ex.Source}", "OK");
        }
    }
    
}