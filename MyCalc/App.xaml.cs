using Calculation;
using Microsoft.Extensions.DependencyInjection;
using MyCalc.ViewModels;
using System;
using System.Windows;
using MyCalc.Services;
using CalcAppShared.Services;

namespace MyCalc;
/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private readonly ServiceProvider _serviceProvider;

    public App()
    {
        ServiceCollection services = new();
        try
        {
            ConfigureServices(services);
        }
        catch(Exception ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            Shutdown();
        }
        _serviceProvider = services.BuildServiceProvider();
    }

    private static void ConfigureServices(ServiceCollection services)
    {
        services.AddSingleton<IDialogService, DialogService>();
        services.AddSingleton<MainViewModel>();
        services.AddSingleton<MainWindow>();
    }

    private void OnStartup(object sender, StartupEventArgs e)
    {
        var mainWindow = _serviceProvider.GetService<MainWindow>() ?? throw new NullReferenceException("Exception on dependency injection!");
        mainWindow.Show();
    }
}
