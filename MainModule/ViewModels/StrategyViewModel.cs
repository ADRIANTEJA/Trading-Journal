using API;
using API.Events;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MainModule.DataAccess;
using MainModule.DataModel;
using System.Collections.ObjectModel;
using System.Data.SQLite;

namespace MainModule.ViewModels;

public partial class StrategyViewModel : ObservableObject, IViewModel
{
    private readonly IEventAggregator _eventAggregator;

    private readonly StrategyAccess _strategyAccess;

    private readonly INavigationHelper _navigationHelper;

    public INavigationHelper NavigationHelper => _navigationHelper;

    [ObservableProperty]
    private Strategy selectedStrategy;

    [ObservableProperty]
    private string nameVM;

    [ObservableProperty]
    private string goalVM;

    [ObservableProperty]
    private string marketVM;

    [ObservableProperty]
    private string intermediaryVM;

    [ObservableProperty]
    private double riskRewardRatioVM;

    [ObservableProperty]
    private double maxTradeRiskVM;

    [ObservableProperty]
    private double dailyGoalVM;

    [ObservableProperty]
    private double maxDailyLossVM;

    [ObservableProperty]
    private int winsVM;

    [ObservableProperty]
    private int lossesVM;

    [ObservableProperty]
    private Strategy? selectedStrategyVM;

    public ObservableCollection<Strategy> Strategies { get; } = [];

    public ObservableCollection<StrategyPerformanceDataBundle> StrategyPerformance { get; } = [];

    [RelayCommand]
    private void LoadStrategies()
    {
        Strategies.Clear();

        var tempDataReckords = _strategyAccess.QueryStrategiesAsync().Result;

        foreach (var strategy in tempDataReckords) Strategies.Add(strategy);
    }

    [RelayCommand]
    private void AddStrategy()
    {
        var newStrategy = new Strategy
        {
            Name = NameVM,
            Intermediary = IntermediaryVM,
            Goal = GoalVM,
            RiskRewardRatio = RiskRewardRatioVM,
            MaxTradeRisk = MaxTradeRiskVM,
            DailyGoal = DailyGoalVM,
            MaxDailyLoss = MaxDailyLossVM
        };

        try
        {
            _strategyAccess.InsertStrategy(newStrategy);
            Strategies.Add(newStrategy);
            _eventAggregator.GetEvent<CreateStrategyEvent>().Publish(true);
        }
        catch (SQLiteException) { _eventAggregator.GetEvent<CreateStrategyEvent>().Publish(false); }
    }

    [RelayCommand]
    private void UpdateStrategyWonTrades(string strategyName)
    {
        if (_strategyAccess.UpdateStrategyWonTrades(strategyName) > 0) LoadStrategies();
    }

    [RelayCommand]
    private void UpdateStrategyLostTrades(string strategyName)
    {
        if (_strategyAccess.UpdateStrategyLostTrades(strategyName) > 0) LoadStrategies();
    }

    public StrategyViewModel(IEventAggregator eventAggregator,
                             StrategyAccess strategyAccess,
                             INavigationHelper navigationHelper)
    {
        _eventAggregator = eventAggregator;
        _strategyAccess = strategyAccess;
        _navigationHelper = navigationHelper;

        _eventAggregator.GetEvent<LoadAnalysisNotesClickEvent>().Subscribe(OpenAnalysisNotesHandler);
        _eventAggregator.GetEvent<SelectedStrategyItemChangedEvent>().Subscribe(UpdateSelectedStrategyHandler);
        _eventAggregator.GetEvent<StrategyPerformanceDataRequiredEvent>().Subscribe(ProcessStrategyPerformanceHandler);
    }

    private void OpenAnalysisNotesHandler()
    {
        //I used dynamic typing here because this proyect can't see the implementation type of
        //the mainNavigationHelper menber just its interface type, thus I needed to avoid the
        //compile time check for the NavigateToTradeImagesCommand Property of the MainNavigationHelper
        //implementation
        dynamic navHelperRef = _navigationHelper;
        navHelperRef.NavigateToAnalysisNotesCommand.Execute(null);
    }

    private void UpdateSelectedStrategyHandler(object strategy) => SelectedStrategy = (Strategy)strategy;

    private void ProcessStrategyPerformanceHandler(List<StrategyPerformanceDataBundle> strategyPerformance)
    {
        foreach (var performance in strategyPerformance) StrategyPerformance.Add(performance);
    }
}
