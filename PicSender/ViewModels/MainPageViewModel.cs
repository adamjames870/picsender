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

    private IEmail _email;

    public MainPageViewModel(PicDatabase db, IEmail email) : base(db)
    {
        // PictureGroups = SampleData.GetSampleData();
        var list = Task.Run(() => database.GetPictureGroupsAsync()).Result;
        PictureGroups = new ObservableCollection<PictureGroup>(list);
        _email = email;
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

    [RelayCommand]
    async Task SendEmailAsync(PictureGroup pictureGroup)
    {
        if (_email.IsComposeSupported)
        {
            try
            {
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
    async Task DeletePictureGroupAsync(PictureGroup pictureGroup)
    {
        if (pictureGroup is null) return;

        try
        {
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