using PicSender.Views;

namespace PicSender;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        Routing.RegisterRoute(nameof(PictureGroupDetailView), typeof(PictureGroupDetailView));
    }
}