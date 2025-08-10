using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using CalcAppShared.DependencyInjection;
using MauiCalc.ViewModels;

namespace MauiCalc;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder()
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });
#if DEBUG
		builder.Logging.AddDebug();
#endif

        var services = builder.Services;
        services.AddSmartCalc();

        services.AddSingleton<MainPage>();
        services.AddSingleton<MainViewModel>();


#if WINDOWS
#elif ANDROID
#elif IOS || MACCATALYST
#endif

        return builder.Build();
    }
}
