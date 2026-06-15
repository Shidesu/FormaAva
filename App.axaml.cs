using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core;
using Avalonia.Data.Core.Plugins;
using System.Linq;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using Sentinel.Services;
using Sentinel.ViewModels;
using Sentinel.Views;

namespace Sentinel;

public partial class App : Application
{
    private ServiceProvider _provider = null!;

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        var serviceCollection = new ServiceCollection();

        serviceCollection.AddSingleton<MainViewModel>();
        serviceCollection.AddSingleton<HomeViewModel>();
        serviceCollection.AddSingleton<IEquipmentService, MockEquipmentService>();

        _provider = serviceCollection.BuildServiceProvider();

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow
            {
                DataContext = _provider.GetService<MainViewModel>(),
            };

            desktop.Exit += (_, _) => { _provider.Dispose(); };
        }

        base.OnFrameworkInitializationCompleted();
    }
}