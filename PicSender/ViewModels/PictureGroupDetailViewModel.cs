using System.Collections.ObjectModel;
using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PicSender.Models;
using PicSender.Services;
using PicSender.ViewModels.ItemModels;

namespace PicSender.ViewModels;

[QueryProperty(nameof(PictureGroup), "PictureGroup")]
public partial class PictureGroupDetailViewModel(IMediaPicker mediaPicker, PicDatabase db) : BaseViewModel
{
    [ObservableProperty]
    private PictureGroup _pictureGroup = null!;

    [ObservableProperty]
    private ObservableCollection<PictureItemModel> _pictures = null!;

    public async Task LoadPicturesAsync()
    {
        try
        {
            var pics = await db.GetPicturesAsync(PictureGroup.Id);
            var picModels = pics.Select(p => p.ToPictureItemModel()).ToList();
            Pictures = new ObservableCollection<PictureItemModel>(picModels);
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
            var name = await GetImageName();
            var singlePicture = new SinglePicture { Name = name, FullPath = picture.FullPath, PictureGroupId = PictureGroup.Id };
            Pictures.Add(singlePicture.ToPictureItemModel());
            await db.AddPictureAsync(singlePicture);
            if (PictureGroup.ThumbnailPath is null)
            {
                await UpdatePictureGroupAsync(singlePicture.FullPath);
            }
            else
            {
                await UpdatePictureGroupAsync();
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error: {ex.Message}");
            await Shell.Current.DisplayAlert("Error", $"Exception: {ex.Message}", "OK");
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
            var name = await GetImageName();
            if (name is null) return;
            var localFilePath = Path.Combine(FileSystem.CacheDirectory, name + Path.GetExtension(picture.FileName));
            await using var sourceStream = await picture.OpenReadAsync();
            await using var localFileStream = File.OpenWrite(localFilePath);
            await sourceStream.CopyToAsync(localFileStream);
            var singlePicture = new SinglePicture { Name = name, FullPath = localFilePath, PictureGroupId = PictureGroup.Id };
            Pictures.Add(singlePicture.ToPictureItemModel());
            await db.AddPictureAsync(singlePicture);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error: {ex.Message}");
            await Shell.Current.DisplayAlert("Error", $"Exception: {ex.Message}", "OK");
        }
    }

    private async Task<string?> GetImageName()
    {
        return await App.Current.MainPage.DisplayPromptAsync("Name", "Enter a name for the picture");
    }
    
    [RelayCommand]
    private async Task DeletePictureAsync(PictureItemModel picture)
    {
        try
        {
            string newThumbnail = "XXX";
            if (picture.FullPath == PictureGroup.ThumbnailPath)
            {
                newThumbnail = Pictures.FirstOrDefault()?.FullPath ?? string.Empty;
            } 
            Pictures.Remove(picture);
            await db.DeletePictureAsync(picture.GetPicture());
            if (newThumbnail != "XXX")
            {
                await UpdatePictureGroupAsync(newThumbnail);
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error: {ex.Message}");
            await Shell.Current.DisplayAlert($"Error in {nameof(DeletePictureAsync)}", $"Exception: {ex.Message}, {ex.Source}", "OK");
        }
    }

    [RelayCommand]
    private async Task RenamePictureAsync(PictureItemModel picture)
    {
        try
        {
            var newName = await Shell.Current.DisplayPromptAsync("Name", "Enter the name for the picture", "OK", "Cancel", picture.Name);
            if (!string.IsNullOrEmpty(newName) && newName != picture.Name)
            {
                var newPic = picture.ChangeName(newName);
                await db.UpdatePictureAsync(newPic);
            }
            
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error: {ex.Message}");
            await Shell.Current.DisplayAlert($"Error in {nameof(RenamePictureCommand)}", $"Exception: {ex.Message}, {ex.Source}", "OK");
        }
    }
    
    private async Task UpdatePictureGroupAsync()
    {
        try
        {
            PictureGroup.PictureCount = Pictures.Count;
            await db.UpdatePictureGroupAsync(PictureGroup);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error: {ex.Message}");
            await Shell.Current.DisplayAlert($"Error in {nameof(UpdatePictureGroupAsync)}", $"Exception: {ex.Message}, {ex.Source}", "OK");
        }
    }

    private async Task UpdatePictureGroupAsync(string newThumbnailPath)
    {
        PictureGroup.ThumbnailPath = newThumbnailPath;
        await UpdatePictureGroupAsync();
    }
}