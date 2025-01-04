using System.Collections.ObjectModel;
using System.Diagnostics;
using CommunityToolkit.Mvvm.Input;
using PicSender.Models;
using PicSender.Services;
using PicSender.Views;

namespace PicSender.ViewModels;

public partial class MainPageViewModel : BaseViewModel
{
    public ObservableCollection<PictureGroup> PictureGroups { get; } = [];
    
    public MainPageViewModel()
    {
        PictureGroups = SampleData.GetSampleData();
    }

    [RelayCommand]
    async Task GoToDetailsAsync(PictureGroup pictureGroup)
    {
        if (pictureGroup is null) return;
        
        try
        {
            await Shell.Current.GoToAsync($"{nameof(PictureGroupDetailView)}", true,
                new Dictionary<string, object> { { nameof(PictureGroup), pictureGroup } });
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error: {ex.Message}");
            await Shell.Current.DisplayAlert("Error", $"Exception: {ex.Message}", "OK");
        }
    }
    
}