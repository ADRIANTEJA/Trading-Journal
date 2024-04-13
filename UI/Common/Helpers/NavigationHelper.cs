using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using UI.Services;
using System.Windows.Input;
using MainModule.ViewModels;
using MainModule.Common.Utils;
using Microsoft.Extensions.DependencyInjection;
using UI.Windows;
using MainModule.Common;

namespace UI.Common.Helpers;
/// <summary>
/// Encapsulates an INavigation instance and defines the navigation commands
/// </summary>
public partial class NavigationHelper : ObservableObject
{
    [ObservableProperty]
    private INavigationService _navigation;

    [RelayCommand]
    public void NavigateToAddTrade()
    {
        var addTradeWindow = App.AppHost!.Services.GetRequiredService<AddTradeWindow>();
        addTradeWindow.ShowDialog();
    }

    [RelayCommand]
    public void NavigateToAddAccount()
    {
        var addAccountWindow = App.AppHost!.Services.GetRequiredService<AddAccountWindow>();
        addAccountWindow.ShowDialog();
    }

    private ICommand? navigateToHomeCommand;

    public ICommand NavigateToHomeCommand => navigateToHomeCommand ??= new DelegateCommand
       (o => Navigation.NavigateToAsync<HomeViewModel>(), CanNavigateToHome);

    private ICommand? navigateToAccountCommand;

    public ICommand NavigateToAccountCommand => navigateToAccountCommand ??= new DelegateCommand
        (o => Navigation.NavigateToAsync<AccountViewModel>(), CanNavigateToAccount);

    public ICommand? navigateToStrategyCommand;

    public ICommand? NavigateToStrategyCommand => navigateToStrategyCommand ??= new DelegateCommand
        (o => Navigation.NavigateToAsync<StrategyViewModel>(), CanNavigateToSrategy);

    public bool CanNavigateToHome(object? parameter) => Flags.IsThreadSafe;

    public bool CanNavigateToAccount(object? parameter) => Flags.IsThreadSafe;

    public bool CanNavigateToSrategy(object? parameter) => Flags.IsThreadSafe;

    public NavigationHelper(INavigationService navigation)
    {
        Navigation = navigation;
        Navigation.NavigateToAsync<HomeViewModel>(); 
    }
}
