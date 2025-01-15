using PicSender.ViewModels;

namespace PicSender.Views;

public partial class MainPage : ContentPage
{
 
    public MainPage(MainPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        var vm = BindingContext as MainPageViewModel;
        await vm.LoadPictureGroups();
    }
    
}