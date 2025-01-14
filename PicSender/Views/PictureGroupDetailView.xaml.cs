using PicSender.ViewModels;

namespace PicSender.Views;

public partial class PictureGroupDetailView : ContentPage
{
    public PictureGroupDetailView(PictureGroupDetailViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
    
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        var vm = BindingContext as PictureGroupDetailViewModel;
        await vm.LoadPicturesAsync();
    }
    
}