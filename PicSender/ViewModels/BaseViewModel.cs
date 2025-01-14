using CommunityToolkit.Mvvm.ComponentModel;
using PicSender.Services;

namespace PicSender.ViewModels;

public abstract partial class BaseViewModel(PicDatabase db) : ObservableObject
{

    protected PicDatabase database = db;
    
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsNotBusy))]
    private bool _isBusy;

    [ObservableProperty]
    private string? _title;

    public bool IsNotBusy => !IsBusy;
}
