using CommunityToolkit.Mvvm.ComponentModel;
using PicSender.Services;

namespace PicSender.ViewModels;

public abstract partial class BaseViewModel() : ObservableObject
{
    
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsNotBusy))]
    private bool _isBusy;

    [ObservableProperty]
    private string? _title;

    public bool IsNotBusy => !IsBusy;
}
