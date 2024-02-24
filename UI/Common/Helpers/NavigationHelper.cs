using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using UI.Services;
using System.Windows.Input;
using MainModule.ViewModels;
using MainModule.Common.Helpers;
using Microsoft.Extensions.DependencyInjection;
using UI.Windows;

namespace UI.Common.Helpers;

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

    private ICommand? navigateToHomeCommand;

    public ICommand NavigateToHomeCommand => navigateToHomeCommand ??= new DelegateCommand
        (o => Navigation.NavigateTo<HomeViewModel>());

    private ICommand? navigateToAccountCommand;

    public ICommand NavigateToAccountCommand => navigateToAccountCommand ??= new DelegateCommand
        (o => Navigation.NavigateTo<AccountViewModel>());

    public ICommand? navigateToStrategyCommand;

    public ICommand? NaviagteToStrategyCommand => navigateToStrategyCommand ??= new DelegateCommand
        (o => Navigation.NavigateTo<StrategyViewModel>());

    public NavigationHelper(INavigationService navigation)
    {
        Navigation = navigation;
    }
}
