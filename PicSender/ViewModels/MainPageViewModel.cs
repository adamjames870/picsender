using System.Collections.ObjectModel;
using System.Diagnostics;
using CommunityToolkit.Mvvm.Input;
using PicSender.Models;
using PicSender.Services;
using PicSender.Views;


namespace PicSender.ViewModels;

public partial class MainPageViewModel : BaseViewModel
{
    public ObservableCollection<PictureGroup> PictureGroups { get; }
    
    public MainPageViewModel(PicDatabase db) : base(db)
    {
        // PictureGroups = SampleData.GetSampleData();
        var list = Task.Run(() => database.GetPictureGroupsAsync()).Result;
        PictureGroups = new ObservableCollection<PictureGroup>(list);
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
    
    [RelayCommand]
    async Task AddPictureGroupAsync()
    {
        var title = await Shell.Current.DisplayPromptAsync("Title", "Enter a title for the new group");
        if (string.IsNullOrWhiteSpace(title)) return;
        
        var newGroup = new PictureGroup { Title = title };
        PictureGroups.Add(newGroup);
        await database.AddPictureGroupAsync(newGroup);
    }
    
}