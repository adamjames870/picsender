using System.Collections.ObjectModel;
using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PicSender.Models;
using PicSender.Services;
using PicSender.ViewModels.ItemModels;
using PicSender.Views;


namespace PicSender.ViewModels;

public partial class MainPageViewModel : BaseViewModel
{

    private readonly PicDatabase database;
    
    public ObservableCollection<PictureGroup> PictureGroups { get; }
    
    [ObservableProperty] 
    public ObservableCollection<PictureGroupItemModel> _pictureGroupItems;

    private readonly IEmail _email;

    public MainPageViewModel(PicDatabase db, IEmail email)
    {
        database = db;
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
        PictureGroupItems.Add(new PictureGroupItemModel(newGroup));
        await database.AddPictureGroupAsync(newGroup);
    }

    [RelayCommand]
    async Task SendEmailAsync(PictureGroupItemModel pictureGroupItemModel)
    {

        var options = await database.GetAppOptionsAsync();
        if (string.IsNullOrWhiteSpace(options.EmailAddress))
        {
            await Shell.Current.DisplayAlert("Email Address", "Set your email address", "OK");
            await ChangeEmail();
            return;
        }
        
        if (_email.IsComposeSupported)
        {
            try
            {
                var pictureGroup = await database.GetPictureGroupAsync(pictureGroupItemModel.PictureGroupId);
                using var emailService = new Emails(_email, database);
                var pictures = await database.GetPicturesAsync(pictureGroup.Id);
                await emailService.SendEmailWithAttachaments(pictureGroup, pictures);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error: {ex.Message}");
                await Shell.Current.DisplayAlert("Error", $"Exception: {ex.Message}", "OK");
            }
        }
    }
    
    [RelayCommand]
    async Task DeletePictureGroupAsync(PictureGroupItemModel pictureGroupItemModel)
    {
        try
        {
            PictureGroupItems.Remove(pictureGroupItemModel);
            await database.DeletePictureGroupAsync(pictureGroupItemModel.PictureGroupId);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error: {ex.Message}");
            await Shell.Current.DisplayAlert($"Error in {nameof(DeletePictureGroupCommand)}", $"Exception: {ex.Message}, {ex.Source}", "OK");
        }
    }

    [RelayCommand]
    async Task RenamePictureGroupAsync(PictureGroupItemModel pictureGroupItemModel)
    {
        try
        {
            // var pictureGroup = await database.GetPictureGroupAsync(pictureGroupItemModel.PictureGroupId);
            var newTitle = await Shell.Current.DisplayPromptAsync("Title", "Enter the title for the group", "OK", "Cancel", pictureGroupItemModel.Title);
            if (!string.IsNullOrEmpty(newTitle) && newTitle != pictureGroupItemModel.Title)
            {
                var newModel = pictureGroupItemModel.ChangeTitle(newTitle);
                await database.UpdatePictureGroupAsync(newModel);
            }
            
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error: {ex.Message}");
            await Shell.Current.DisplayAlert($"Error in {nameof(RenamePictureGroupAsync)}", $"Exception: {ex.Message}, {ex.Source}", "OK");
        }
    }

    [RelayCommand]
    async Task ChangeEmail()
    {
        try
        {
            await Shell.Current.GoToAsync($"{nameof(OptionsView)}", true);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error: {ex.Message}");
            await Shell.Current.DisplayAlert($"Error in {nameof(ChangeEmail)}", $"Exception: {ex.Message}, {ex.Source}", "OK");
        }
    }
    
}