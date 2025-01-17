using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PicSender.Models;
using PicSender.Services;

namespace PicSender.ViewModels;

public partial class OptionsViewModel : BaseViewModel
{

    private readonly PicDatabase _database;
    private readonly AppOptions _appOptions;

    [ObservableProperty] 
    private string _emailAddress;

    public OptionsViewModel(PicDatabase db)
    {
        _database = db;
        _appOptions = Task.Run(() => _database.GetAppOptionsAsync()).Result;
        EmailAddress = _appOptions.EmailAddress;
    }

    [RelayCommand]
    private async Task SaveAsync()
    {
        _appOptions.EmailAddress = EmailAddress;
        await _database.SaveAppOptionsAsync(_appOptions);
        await Shell.Current.GoToAsync("..");
    }
    
    [RelayCommand]
    private async Task CancelAsync()
    {
        await Shell.Current.GoToAsync("..");
    }
    
}