using API;
using API.Events;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MainModule.Common.Utils;
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
    private string riskRewardRatioVM;

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
    private async Task LoadStrategies()
    {
        Strategies.Clear(); 

        var tempDataReckords = await _strategyAccess.QueryStrategiesAsync();

        foreach (var strategy in tempDataReckords) Strategies.Add(strategy);

        if (Strategies.Count > 0) SelectedStrategy = Strategies.First();
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
            _ = LoadStrategies();
            _eventAggregator.GetEvent<CreateStrategyEvent>().Publish(true);
        }
        catch (SQLiteException ex) 
        {
            _eventAggregator.GetEvent<CreateStrategyEvent>().Publish(false); 
        }
    }

    [RelayCommand]
    private void UpdateStrategyWonTrades(string strategyName)
    {
        if (_strategyAccess.UpdateStrategyWonTrades(strategyName) > 0) _ = LoadStrategies();
    }

    [RelayCommand]
    private void UpdateStrategyLostTrades(string strategyName)
    {
        if (_strategyAccess.UpdateStrategyLostTrades(strategyName) > 0) _ = LoadStrategies();
    }

    private IMultiParameterCommand updateStrategyCommand;

    public IMultiParameterCommand UpdateStrategyCommand
    {
        get
        {
            if (updateStrategyCommand == null)
            {
                updateStrategyCommand = new MultiparameterDelegateCommand(UpdateStrategy);
            }
            return updateStrategyCommand;
        }
    }

    private void UpdateStrategy(object parameter1, object parameter2)
    {
        var updatedStrategy = (Strategy)parameter1;
        string formerStrategyName = parameter2.ToString()!;

        _strategyAccess.UpdateStrategy(updatedStrategy);

        _eventAggregator.GetEvent<StrategyUpdatedEvent>().Publish(new()
        {
            NewStrategyName = updatedStrategy.Name,
            FormerStrategyName = formerStrategyName
        });

        _ = LoadStrategies();
    }

    public StrategyViewModel(IEventAggregator eventAggregator,
                             StrategyAccess strategyAccess,
                             INavigationHelper navigationHelper)
    {
        _eventAggregator = eventAggregator;
        _strategyAccess = strategyAccess;
        _navigationHelper = navigationHelper;

        _eventAggregator.GetEvent<LoadAnalysisNotesEvent>().Subscribe(OpenAnalysisNotesHandler);
        _eventAggregator.GetEvent<EditStrategyEvent>().Subscribe(OpenEditStrategyWindowHandler);
        _eventAggregator.GetEvent<SelectedStrategyItemChangedEvent>().Subscribe(UpdateSelectedStrategyHandler);
        _eventAggregator.GetEvent<StrategyPerformanceDataRequiredEvent>().Subscribe(ProcessStrategyPerformanceHandler);
        _eventAggregator.GetEvent<DeleteStrategyClickEvent>().Subscribe(DeleteStrategyEventHandler);
    }

    private void DeleteStrategyEventHandler(int strategyId)
    {
        var deletedStrategy = Strategies.First(strategy => strategy.Id == strategyId);

        _eventAggregator.GetEvent<StrategyDeletedEvent>().Publish(new()
        {
            StrategyId = deletedStrategy.Id,
            FormerStrategyName = deletedStrategy.Name
        });

        _ = LoadStrategies();

        if (Strategies.Count == 0) SelectedStrategy = new();
    }

    public void DeleteStrategy(int strategyId) => _strategyAccess.DeleteStrategy(strategyId);

    private void OpenAnalysisNotesHandler()
    {
        //I used dynamic typing here because this proyect can't see the implementation type of
        //the mainNavigationHelper menber just its interface type, thus I needed to avoid the
        //compile time check for the NavigateToTradeImagesCommand Property of the MainNavigationHelper
        //implementation
        dynamic navHelperRef = _navigationHelper;
        navHelperRef.NavigateToAnalysisNotesCommand.Execute(null);
    }

    private void OpenEditStrategyWindowHandler()
    {
        dynamic navHelperRef = _navigationHelper;
        navHelperRef.NavigateToEditStrategyCommand.Execute(null);
    }

    private void UpdateSelectedStrategyHandler(object strategy) => SelectedStrategy = (Strategy)strategy;

    private void ProcessStrategyPerformanceHandler(List<StrategyPerformanceDataBundle> strategyPerformance)
    {
        foreach (var performance in strategyPerformance) StrategyPerformance.Add(performance);
    }
}
