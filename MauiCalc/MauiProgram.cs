using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using CalcAppShared.DependencyInjection;
using MauiCalc.ViewModels;

namespace MauiCalc;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        var services = builder.Services;

        services.AddSingleton<MainPage>();
        services.AddSingleton<MainViewModel>();

#if DEBUG
		builder.Logging.AddDebug();
#endif

#if WINDOWS
#elif ANDROID
#elif IOS || MACCATALYST
#endif

        return builder.Build();
    }
}
