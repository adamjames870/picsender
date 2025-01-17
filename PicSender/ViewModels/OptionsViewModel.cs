using CommunityToolkit.Mvvm.ComponentModel;
using PicSender.Models;

namespace PicSender.ViewModels;

[QueryProperty(nameof(AppOptions), "AppOptions")]
public partial class OptionsViewModel: BaseViewModel
{
    [ObservableProperty]
    private AppOptions _appOptions = new AppOptions { EmailAddress = string.Empty };
    
    public string EmailAddress
    {
        get => AppOptions.EmailAddress;
        set
        {
            AppOptions.EmailAddress = value;
            OnPropertyChanged();
        }
    }
}