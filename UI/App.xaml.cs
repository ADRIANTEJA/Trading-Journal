using UI.Services;
using MainModule.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Windows;
using UI.Views;
using UI.Windows;
using UI.Common.Helpers;
using MainModule.DataAccess;
using MainModule.Services;
using System.IO;
using MainModule.Common;
using API;
using Prism.Events;

namespace UI;

public partial class App : Application 
{
    public static IHost? AppHost { get; private set; }
    private static readonly string appDirectoryPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), 
                                                                   Constants.ApplicationDataFolderName);
    public App()
    {
        ConfigureServices();
    }

    protected override async void OnStartup(StartupEventArgs e)
    {
        await AppHost!.StartAsync();

        if (!Directory.Exists(appDirectoryPath)) { Directory.CreateDirectory(appDirectoryPath); }

        var startPoint = AppHost.Services.GetRequiredService<MainWindow>();
        startPoint.Show();
        
        base.OnStartup(e);
    }

    protected override async void OnExit(ExitEventArgs e)
    {
        await AppHost!.StopAsync();

        base.OnExit(e);
    }

    private void ConfigureServices()
    {
        AppHost = Host.CreateDefaultBuilder()
            .ConfigureServices((hostContext, services) =>
            {
                //Hard Dependencies
                services.AddSingleton<IEventAggregator, EventAggregator>();
                services.AddSingleton<INavigationService, NavigationService>();
                services.AddTransient<INavigationHelper, MainNavigationHelper>();
                services.AddSingleton<IConfigurationService, ConfigurationService>();
                services.AddSingleton<IUIConfigurationService, UIConfigurationService>();
                services.AddSingleton<Func<Type, IViewModel>>
                    (provider => viewModelType => (IViewModel)provider.GetRequiredService(viewModelType));
                //Windows
                services.AddSingleton(provider => new MainWindow(provider.GetRequiredService<IUIConfigurationService>())
                {
                    DataContext = provider.GetRequiredService<INavigationHelper>()
                });
                services.AddTransient(provider => new SelectLanguageWindow(provider.GetRequiredService<IUIConfigurationService>()));
                services.AddTransient(provider => new AddTradeWindow
                {
                    DataContext = provider.GetRequiredService<HomeViewModel>()
                });
                services.AddTransient(provider => new AddAccountWindow(provider.GetRequiredService<IEventAggregator>())
                {
                    DataContext = provider.GetRequiredService<AccountViewModel>()
                });
                //Views
                services.AddTransient(provider => new StrategyView
                {
                    DataContext = provider.GetRequiredService<StrategyViewModel>()
                });
                services.AddTransient(provider => new AccountView
                {
                    DataContext = provider.GetRequiredService<AccountViewModel>()
                });
                services.AddTransient(provider => new HomeView
                {
                    DataContext = provider.GetRequiredService<HomeViewModel>()
                });
                //Data Access
                services.AddTransient<AccountAccess>();
                services.AddTransient<TradeAccess>();
                services.AddTransient<DayPerformanceAccess>();
                //ViewModels
                services.AddSingleton<StrategyViewModel>();
                services.AddSingleton<SymbolViewModel>();
                services.AddSingleton<TradeImageViewModel>();
                services.AddSingleton<DayPerformanceViewModel>();
                services.AddSingleton<AnalysisNoteViewModel>();
                services.AddSingleton(provider => new HomeViewModel(provider.GetRequiredService<AccountViewModel>(),
                                                                    provider.GetRequiredService<TradeAccess>()));

                services.AddSingleton(provider => new AccountViewModel(provider.GetRequiredService<AccountAccess>(),
                                                                       provider.GetRequiredService<DayPerformanceAccess>(),
                                                                       provider.GetRequiredService<INavigationHelper>(),
                                                                       provider.GetRequiredService<IEventAggregator>()));
            }).Build();
    }
}
