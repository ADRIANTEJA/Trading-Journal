using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using UI.Services;
using System.Windows.Input;
using MainModule.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using UI.Windows;
using MainModule.Common;
using MainModule.Common.Utils;
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
        var tradeMistakesWindow = App.AppHost!.Services.GetRequiredService<TradeMistakesWindow>();
        tradeMistakesWindow.ShowDialog();
    }

    [RelayCommand]
    private void NavigateToTradeCosts()
    {
        var tradeCostsWindow = App.AppHost!.Services.GetRequiredService<TradeCostsWindow>();
        tradeCostsWindow.ShowDialog();
    }

    [RelayCommand]
    private void NavigateToPortfolio()
    {
        var portfolioWindow = App.AppHost!.Services.GetRequiredService<PortfolioWindow>();
        portfolioWindow.ShowDialog();
    }

    [RelayCommand]
    private void NavigateToAddSymbol()
    {
        var addSymbolWindow = App.AppHost!.Services.GetRequiredService<AddSymbolWindow>();
        addSymbolWindow.ShowDialog();
    }

    [RelayCommand]
    private void NavigateToAddStrategy()
    {
        var addStrategyWindow = App.AppHost!.Services.GetRequiredService<AddStrategyWindow>();
        addStrategyWindow.ShowDialog();
    }

    [RelayCommand]
    private void NavigateToAnalysisNotes()
    {
        var analysisNotesWindow = App.AppHost!.Services.GetRequiredService<AnalysisNotesWindow>();
        analysisNotesWindow.ShowDialog();
    }

    [RelayCommand]
    private void NavigateToAddAnalysisNote()
    {
        var addAnalysisNoteWindow = App.AppHost!.Services.GetRequiredService<AddAnalysisNoteWindow>();
        addAnalysisNoteWindow.ShowDialog();
    }

    [RelayCommand]
    private void NavigateToEditTrade()
    {
        var editTradeWindow = App.AppHost!.Services.GetRequiredService<EditTradeWindow>();
        editTradeWindow.ShowDialog();
    }

    [RelayCommand]
    private void NavigateToEditStrategy()
    {
        var editStrategyWindow = App.AppHost!.Services.GetRequiredService<EditStrategyWindow>();
        editStrategyWindow.ShowDialog();
    }

    private ICommand? navigateToHomeCommand;

    public ICommand NavigateToHomeCommand => navigateToHomeCommand ??= new MainModule.Common.Utils.DelegateCommand
       (o => Navigation.NavigateToAsync<HomeViewModel>(), CanNavigateToHome);

    private ICommand? navigateToAccountCommand;

    public ICommand NavigateToAccountCommand => navigateToAccountCommand ??= new MainModule.Common.Utils.DelegateCommand
        (o => Navigation.NavigateToAsync<AccountViewModel>(), CanNavigateToAccount);

    public ICommand? navigateToStrategyCommand;

    public ICommand? NavigateToStrategyCommand => navigateToStrategyCommand ??= new MainModule.Common.Utils.DelegateCommand
        (o => Navigation.NavigateToAsync<StrategyViewModel>(), CanNavigateToSrategy);

    private bool CanNavigateToHome(object? parameter) => Flags.IsThreadSafe;

    private bool CanNavigateToAccount(object? parameter) => Flags.IsThreadSafe;

    private bool CanNavigateToSrategy(object? parameter) => Flags.IsThreadSafe;

    public MainNavigationHelper(INavigationService navigation)
    {
        Navigation = navigation;
    }
}
