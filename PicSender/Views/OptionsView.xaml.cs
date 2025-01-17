using PicSender.ViewModels;

namespace PicSender.Views;

public partial class OptionsView : ContentPage
{
    public OptionsView(OptionsViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}