using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using UI.Services;
using System.Windows.Input;
using MainModule.ViewModels;
using MainModule.Common.Utils;
using Microsoft.Extensions.DependencyInjection;
using UI.Windows;
using MainModule.Common;
using API;

namespace UI.Common.Helpers;
/// <summary>
/// Encapsulates an INavigationService instance and defines the navigation commands
/// </summary>
public partial class MainNavigationHelper : ObservableObject, INavigationHelper
{
    [ObservableProperty]
    private INavigationService _navigation;

    [RelayCommand]
    private void NavigateToAddTrade()
    {
        var addTradeWindow = App.AppHost!.Services.GetRequiredService<AddTradeWindow>();
        addTradeWindow.ShowDialog();
    }

    [RelayCommand]
    private void NavigateToAddAccount()
    {
        var addAccountWindow = App.AppHost!.Services.GetRequiredService<AddAccountWindow>();
        addAccountWindow.ShowDialog();
    }

    [RelayCommand]
    private void NavigateToSelectLanguage()
    {
        var selectLanguageWindow = App.AppHost!.Services.GetRequiredService<SelectLanguageWindow>();
        selectLanguageWindow.ShowDialog();
    }

    [RelayCommand]
    private void NavigateToTradeImages()
    {
        var tradeImagesWindow = App.AppHost!.Services.GetRequiredService<TradeImageWindow>();
        tradeImagesWindow.ShowDialog();
    }

    [RelayCommand]
    private void NavigateToTradeNotes()
    {
        var tradeNotesWindow = App.AppHost!.Services.GetRequiredService<TradeNotesWindow>();
        tradeNotesWindow.ShowDialog();
    }

    [RelayCommand]
    private void NavigateToTradeMistakes()
    {
        var tradeMistakesWindow = new TradeMistakesWindow();
        tradeMistakesWindow.ShowDialog();
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

    private bool CanNavigateToHome(object? parameter) => Flags.IsThreadSafe;

    private bool CanNavigateToAccount(object? parameter) => Flags.IsThreadSafe;

    private bool CanNavigateToSrategy(object? parameter) => Flags.IsThreadSafe;

    public MainNavigationHelper(INavigationService navigation)
    {
        Navigation = navigation;
    }
}
