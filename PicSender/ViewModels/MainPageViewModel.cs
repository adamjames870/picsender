using System.Collections.ObjectModel;
using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PicSender.Models;
using PicSender.Services;
using PicSender.Views;


namespace PicSender.ViewModels;

public partial class MainPageViewModel : BaseViewModel
{
    public ObservableCollection<PictureGroup> PictureGroups { get; }
    
    [ObservableProperty] 
    private ObservableCollection<PictureGroupItemModel> _pictureGroupItems;

    private IEmail _email;

    public MainPageViewModel(PicDatabase db, IEmail email) : base(db)
    {
        // PictureGroups = SampleData.GetSampleData();
        var list = Task.Run(() => database.GetPictureGroupsAsync()).Result;
        PictureGroups = new ObservableCollection<PictureGroup>(list);
        _email = email;
    }

    public async Task LoadPictureGroups()
    {
        try
        {
            var list = await database.GetPictureGroupsAsync();
            var groups = list.Select(group => group.ToPictureGroupItemModel()).ToList();
            PictureGroupItems = new ObservableCollection<PictureGroupItemModel>(groups);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error: {ex.Message}");
            await Shell.Current.DisplayAlert(nameof(LoadPictureGroups), $"Exception: {ex.Message}", "OK");
        }
    }

    [RelayCommand]
    async Task GoToDetailsAsync(PictureGroupItemModel pictureGroupItemModel)
    {
        if (pictureGroupItemModel is null) return;

        try
        {
            var pictureGroup = await database.GetPictureGroupAsync(pictureGroupItemModel.PictureGroupId);
            await Shell.Current.GoToAsync($"{nameof(PictureGroupDetailView)}", true,
                new Dictionary<string, object> { { nameof(PictureGroup), pictureGroup } });
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error: {ex.Message}");
            await Shell.Current.DisplayAlert(nameof(GoToDetailsCommand), $"Exception: {ex.Message}", "OK");
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

    [RelayCommand]
    async Task SendEmailAsync(PictureGroupItemModel pictureGroupItemModel)
    {
        if (_email.IsComposeSupported)
        {
            try
            {
                var pictureGroup = await database.GetPictureGroupAsync(pictureGroupItemModel.PictureGroupId);
                using var emailService = new Emails(_email);
                var pictures = await database.GetPicturesAsync(pictureGroup.Id);
                await emailService.SendEmailWithAttachaments(pictureGroup, pictures);
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
    
    [RelayCommand]
    async Task DeletePictureGroupAsync(PictureGroupItemModel pictureGroupItemModel)
    {
        try
        {
            var pictureGroup = await database.GetPictureGroupAsync(pictureGroupItemModel.PictureGroupId);
            PictureGroups.Remove(pictureGroup);
            await database.DeletePictureGroupAsync(pictureGroup.Id);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error: {ex.Message}");
            await Shell.Current.DisplayAlert($"Error in {nameof(DeletePictureGroupCommand)}", $"Exception: {ex.Message}, {ex.Source}", "OK");
        }
    }
    
}