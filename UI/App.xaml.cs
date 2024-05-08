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

namespace UI;

public partial class App : Application 
{
    public static IHost? AppHost { get; private set; }

    public App()
    {
        ConfigureServices();
    }

    protected override async void OnStartup(StartupEventArgs e)
    {
        await AppHost!.StartAsync();

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
                services.AddSingleton<INavigationService, NavigationService>();
                services.AddTransient<NavigationHelper>();
                services.AddSingleton<IDataAccessConfiguration, DataAccessConfiguration>();
                services.AddSingleton<Func<Type, IViewModel>>
                    (provider => viewModelType => (IViewModel)provider.GetRequiredService(viewModelType));
                //Windows
                services.AddSingleton(provider => new MainWindow
                {
                    DataContext = provider.GetRequiredService<NavigationHelper>()
                });
                services.AddSingleton(provider => new AddTradeWindow
                {
                    DataContext = provider.GetRequiredService<HomeViewModel>()
                }); ;
                services.AddSingleton(provider => new AddAccountWindow
                {
                    DataContext = provider.GetRequiredService<AccountViewModel>()
                });
                //Views
                services.AddTransient(provider => new HomeView
                {
                    DataContext = provider.GetRequiredService<HomeViewModel>()
                });
                services.AddTransient(provider => new AccountView
                {
                    DataContext = provider.GetRequiredService<AccountViewModel>()
                });
                services.AddTransient(provider => new StrategyView 
                { 
                    DataContext = provider.GetRequiredService<StrategyViewModel>() 
                });
                //Data Access
                services.AddTransient<AccountAccess>();
                services.AddTransient<TradeAccess>();
                //ViewModels
                services.AddSingleton<HomeViewModel>();
                services.AddSingleton<AccountViewModel>();
                services.AddSingleton<StrategyViewModel>();
                services.AddTransient<SymbolViewModel>();
                services.AddTransient<TradeImageViewModel>();
                services.AddTransient<DayPerformanceViewModel>();
                services.AddTransient<AnalysisNoteViewModel>();
            }).Build();
    }
}
