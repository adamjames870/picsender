using Microsoft.Extensions.Logging;
using PicSender.Services;
using PicSender.ViewModels;
using PicSender.Views;

namespace PicSender;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        builder.Services.AddSingleton(MediaPicker.Default);

        builder.Services.AddSingleton<PicDatabase>();
        
        builder.Services.AddSingleton<MainPageViewModel>();
        builder.Services.AddTransient<PictureGroupDetailViewModel>();
        
        builder.Services.AddSingleton<MainPage>();
        builder.Services.AddTransient<PictureGroupDetailView>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}