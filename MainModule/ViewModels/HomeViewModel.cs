using API;
using API.Events;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MainModule.Common.Utils;
using MainModule.DataAccess;
using MainModule.DataModel;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SQLite;
using static MainModule.Common.Enums;

namespace MainModule.ViewModels;

public partial class HomeViewModel : ObservableObject, IViewModel
{
    private readonly INavigationHelper _mainNavigationHelper;

    private readonly INavigationHelper _navigationHelper;

    private readonly IEventAggregator _eventAggregator;

    public IEventAggregator EventAggregator => _eventAggregator;

    private readonly AccountViewModel _accountViewModel;

    public AccountViewModel AccountViewModel => _accountViewModel;

    private readonly SymbolViewModel _symbolViewModel;

    public SymbolViewModel SymbolViewModel => _symbolViewModel;

    private readonly TradeAccess _tradeAccess;

    private readonly StrategyViewModel _strategyViewModel;

    public StrategyViewModel StrategyViewModel => _strategyViewModel;

    private readonly PerformanceViewModel _performanceViewModel;

    public PerformanceViewModel PerformanceViewModel => _performanceViewModel;

    public ObservableCollection<Trade> Trades { get; } = [];

    private Trade selectedTrade;

    public Trade SelectedTrade { get; set; }

    [ObservableProperty]
    private DateTime openDateVM;

    [ObservableProperty]
    private DateTime closeDateVM = new DateTime(0);

    [ObservableProperty]
    private double volumeVM;

    [ObservableProperty]
    private double openPriceVM;

    [ObservableProperty]
    private double closePriceVM = 0;

    [ObservableProperty]
    private double swapVM = 0;

    [ObservableProperty]
    private double spreadVM = 0;

    [ObservableProperty]
    private double commissionVM = 0;

    [ObservableProperty]
    private double otherCostsVM = 0;

    [ObservableProperty]
    private double stopLossVM = 0;

    [ObservableProperty]
    private double takeProfitVM = 0;

    [ObservableProperty]
    private string mistakesVM;

    [ObservableProperty]
    private string notesVM;

    [ObservableProperty]
    private int leverageVM = 0;

    [RelayCommand]
    private async Task LoadTrades()
    {
        Trades.Clear();

        if (_accountViewModel.SelectedAccount != null)
        {
            var tempDataReckords = await _tradeAccess.QueryAccountTradesAsync(_accountViewModel.SelectedAccount.Id);

            foreach (var trade in tempDataReckords.OrderBy(trade => trade.OpenDate)) Trades.Add(trade);

            _eventAggregator.GetEvent<StrategyPerformanceDataRequiredEvent>().Publish(GetStrategyPerformanceData());
        }
    }

    private IMultiParameterCommand addTradeCommand;

    public IMultiParameterCommand AddTradeCommand
    {
        get
        {
            if (addTradeCommand == null)
            {
                addTradeCommand = new MultiparameterDelegateCommand(AddTrade);
            }
            return addTradeCommand;
        }
    }

    private void AddTrade(object parameter1, object parameter2)
    {
        var tradeSide = (TradeSide)parameter1;
        var tradeStatus = (TradeStatus)parameter2;

        //The leverage is conditional because the leverage slider can have a bug in wpf
        //where it starts at 0 value
        var trade = new Trade()
        {
            AccountId = _accountViewModel.SelectedAccount.Id,
            PairTraded = SymbolViewModel.SelectedSymbolVM.Pair,
            PairMarket = SymbolViewModel.SelectedSymbolVM.AssetType,
            OpenDate = CloseDateVM.Ticks,
            CloseDate = tradeStatus == TradeStatus.Closed ? CloseDateVM.Ticks : null,
            Side = tradeSide,
            Volume = VolumeVM,
            Status = tradeStatus,
            OpenPrice = OpenPriceVM,
            ClosePrice = tradeStatus == TradeStatus.Closed ? ClosePriceVM : null,
            TradeCost = VolumeVM * OpenPriceVM,
            Swap = SwapVM,
            Spread = SpreadVM,
            Commission = CommissionVM,
            OtherCosts = OtherCostsVM,
            TakeProfit = TakeProfitVM,
            StopLoss = StopLossVM,
            Mistakes = MistakesVM,
            Notes = NotesVM,
            Roi = tradeStatus == TradeStatus.Closed ? CalculateReturnOnInvestment(tradeSide,
                                                                                  VolumeVM,
                                                                                  OpenPriceVM,
                                                                                  ClosePriceVM,
                                                                                  SwapVM,
                                                                                  SpreadVM,
                                                                                  CommissionVM,
                                                                                  OtherCostsVM,
                                                                                  LeverageVM == 0 ? 1 : LeverageVM)[0] : null,

            RoiPercentage = tradeStatus == TradeStatus.Closed ? CalculateReturnOnInvestment(tradeSide,
                                                                                            VolumeVM,
                                                                                            OpenPriceVM,
                                                                                            ClosePriceVM,
                                                                                            SwapVM,
                                                                                            SpreadVM,
                                                                                            CommissionVM,
                                                                                            OtherCostsVM,
                                                                                            LeverageVM == 0 ? 1 : LeverageVM)[1] : null,

            StrategyName = StrategyViewModel.SelectedStrategyVM == null
                           || StrategyViewModel.SelectedStrategyVM.Name.Length < 0 ? null : StrategyViewModel.SelectedStrategyVM.Name,

            Leverage = LeverageVM == 0 ? 1 : LeverageVM,
            AccountBalance = tradeStatus == TradeStatus.Closed ? AccountViewModel.SelectedAccount.CurrentBalance : null
        };

        try
        {
            _tradeAccess.InsertTrade(trade);
            _ = LoadTrades();

            var performance = new Performance();

            if (trade.StrategyName != null 
                && IsWonTrade(trade)
                && tradeStatus == TradeStatus.Closed) _strategyViewModel.UpdateStrategyWonTradesCommand.Execute(trade.StrategyName);

            else if (trade.StrategyName != null
                     && tradeStatus == TradeStatus.Closed) _strategyViewModel.UpdateStrategyLostTradesCommand.Execute(trade.StrategyName);

            _eventAggregator.GetEvent<CreateTradeEvent>().Publish(true);

            if (trade.Status == TradeStatus.Closed)
            {
                performance = new Performance
                {
                    AccountId = AccountViewModel.SelectedAccount.Id,
                    Date = CloseDateVM.Ticks,
                    ROI = CalculateReturnOnInvestment(tradeSide,
                                                  VolumeVM,
                                                  OpenPriceVM,
                                                  ClosePriceVM,
                                                  SwapVM,
                                                  SpreadVM,
                                                  CommissionVM,
                                                  OtherCostsVM,
                                                  LeverageVM == 0 ? 1 : LeverageVM)[0],

                    Cost = (VolumeVM * OpenPriceVM) + SwapVM + SpreadVM + CommissionVM + OtherCostsVM
                };

                _performanceViewModel.AddAccountPerformanceRecord(performance);
                _ = _performanceViewModel.LoadDailyPerformance(_accountViewModel.SelectedAccount.Id);
            }

            AccountViewModel.UpdateAccountBalance(CalculateNewAccountBalance(trade));

            if (trade.StrategyName != null)
                _eventAggregator.GetEvent<StrategyDataRquiredIntermediaryEvent>().Publish();
            CleanVMData();
        }
        catch (SQLiteException ex) 
        {
            _eventAggregator.GetEvent<CreateTradeEvent>().Publish(false); 
        }
    }

    private IMultiParameterCommand updateTradeCommand;

    public IMultiParameterCommand UpdateTradeCommand
    {
        get
        {
            if (updateTradeCommand == null)
            {
                updateTradeCommand = new MultiparameterDelegateCommand(UpdateTrade);
            }
            return updateTradeCommand;
        }
    }

    private void UpdateTrade(object parameter1, object parameter2)
    {
        var updatedTrade = (Trade)parameter1;
        var tradeClosed = (bool)parameter2;

        if (updatedTrade.Status == TradeStatus.Closed)
        {
            _performanceViewModel.DeletePerformanceByDate(updatedTrade.CloseDate!.Value);

            var updatedPerformance = new Performance
            {
                AccountId = _accountViewModel.SelectedAccount.Id,
                Date = (long)updatedTrade.CloseDate,
                ROI = CalculateReturnOnInvestment(updatedTrade.Side,
                                                  updatedTrade.Volume,
                                                  updatedTrade.OpenPrice,
                                                  updatedTrade.ClosePrice!.Value,
                                                  updatedTrade.Swap,
                                                  updatedTrade.Spread,
                                                  updatedTrade.Commission,
                                                  updatedTrade.OtherCosts,
                                                  updatedTrade.Leverage)[0],
                Cost = (updatedTrade.Volume * updatedTrade.OpenPrice) 
                + updatedTrade.Swap + updatedTrade.Spread + updatedTrade.Commission + updatedTrade.OtherCosts
            };

            updatedTrade.Roi = CalculateReturnOnInvestment(updatedTrade.Side,
                                                           updatedTrade.Volume,
                                                           updatedTrade.OpenPrice,
                                                           updatedTrade.ClosePrice!.Value,
                                                           updatedTrade.Swap,
                                                           updatedTrade.Spread,
                                                           updatedTrade.Commission,
                                                           updatedTrade.OtherCosts,
                                                           updatedTrade.Leverage)[0];

            updatedTrade.RoiPercentage = CalculateReturnOnInvestment(updatedTrade.Side,
                                                                     updatedTrade.Volume,
                                                                     updatedTrade.OpenPrice,
                                                                     updatedTrade.ClosePrice!.Value,
                                                                     updatedTrade.Swap,
                                                                     updatedTrade.Spread,
                                                                     updatedTrade.Commission,
                                                                     updatedTrade.OtherCosts,
                                                                     updatedTrade.Leverage)[1];

            _performanceViewModel.AddAccountPerformanceRecord(updatedPerformance);
            _ = _performanceViewModel.LoadDailyPerformance(_accountViewModel.SelectedAccount.Id);

            if (tradeClosed) updatedTrade.AccountBalance = AccountViewModel.SelectedAccount.CurrentBalance;

            _tradeAccess.UpdateTrade(updatedTrade);
            _ = LoadTrades();
        }

        _tradeAccess.UpdateTrade(updatedTrade);
        _ = LoadTrades();
    }

    [RelayCommand]
    private void DeleteTrade(int id)
    {
        if (_tradeAccess.DeleteTrade(id) == 1)
        {
            var deletedTrade = Trades.Where(x => x.Id == id).FirstOrDefault();

            if (deletedTrade!.StrategyName != null)
                _eventAggregator.GetEvent<StrategyDataRquiredIntermediaryEvent>().Publish();

            Trades.Remove(deletedTrade!);

            if (deletedTrade!.CloseDate != null)
            {
                _performanceViewModel.DeletePerformanceByDate(deletedTrade.CloseDate!.Value);
                _ = _performanceViewModel.LoadDailyPerformance(_accountViewModel.SelectedAccount.Id);
            }
        }
    }

    private IMultiParameterCommand filterTradesCommand;

    public IMultiParameterCommand FilterTradesCommand
    {
        get
        {
            if (filterTradesCommand == null)
            {
                filterTradesCommand = new MultiparameterDelegateCommand(FilterTrades);
            }
            return filterTradesCommand;
        }
    }

    private void FilterTrades(object filterKey, object? filterData)
    {
        var key = (FilterKey)filterKey;

        switch (filterKey)
        {
            case FilterKey.Win:
                RemoveLeftOverTrades(Trades.Where(Trade => !IsWonTrade(Trade)).ToList());
                break;
            case FilterKey.Loss:
                RemoveLeftOverTrades(Trades.Where(IsWonTrade).ToList());
                break;
            case FilterKey.Open:
                RemoveLeftOverTrades(Trades.Where(Trade => Trade.Status == TradeStatus.Closed).ToList());
                break;
            case FilterKey.Long:
                RemoveLeftOverTrades(Trades.Where(Trade => Trade.Side == TradeSide.Short).ToList());
                break;
            case FilterKey.Short:
                RemoveLeftOverTrades(Trades.Where(Trade => Trade.Side == TradeSide.Long).ToList());
                break;
            case FilterKey.OpenDate:
                if (filterData == null) return;

                RemoveLeftOverTrades(Trades
                    .Where(Trade => new DateTime(Trade.OpenDate).ToString("dd/MM/yyyy") 
                                    != new DateTime((long)filterData).ToString("dd/MM/yyyy")).ToList());
                break;
            case FilterKey.CloseDate:
                if (filterData == null) return;

                RemoveLeftOverTrades(Trades
                    .Where(Trade => Trade.CloseDate != null
                                    && new DateTime(Trade.CloseDate!.Value).ToString("dd/MM/yyyy")
                                    != new DateTime((long)filterData).ToString("dd/MM/yyyy")).ToList());
                break;
            case FilterKey.Symbol:
                if (filterData == null) return;

                RemoveLeftOverTrades(Trades
                    .Where(Trade => Trade.PairTraded != filterData.ToString()).ToList());
                break;
        }
    }

    public HomeViewModel(AccountViewModel accountViewModel,
                         SymbolViewModel symbolViewModel,
                         StrategyViewModel strategyViewModel,
                         PerformanceViewModel performanceViewModel,
                         TradeAccess tradeAccess,
                         IEventAggregator eventAggregator,
                         INavigationHelper mainNavigationHelper)
    {
        _accountViewModel = accountViewModel;
        _symbolViewModel = symbolViewModel;
        _strategyViewModel = strategyViewModel;
        _performanceViewModel = performanceViewModel;
        _tradeAccess = tradeAccess;
        _mainNavigationHelper = mainNavigationHelper;
        _eventAggregator = eventAggregator;

        _eventAggregator.GetEvent<SelectedTradeItemChangedEvent>().Subscribe(UpdateSelectedTradeHandler);
        _eventAggregator.GetEvent<LoadTradeImagesEvent>().Subscribe(OpenTradeImagesHandler);
        _eventAggregator.GetEvent<LoadTradeNotesEvent>().Subscribe(OpenTradeNotesHandler);
        _eventAggregator.GetEvent<LoadTradeMistakesEvent>().Subscribe(OpenTradeMistakesHandler);
        _eventAggregator.GetEvent<LoadTradeCostsEvent>().Subscribe(OpenTradeCostsHandler);
        _eventAggregator.GetEvent<EditTradeEvent>().Subscribe(OpenEditTradeWindowHandler);
        _eventAggregator.GetEvent<StrategyDataRquiredIntermediaryEvent>().Subscribe(FireRequiredStrategyUsageDataEventHandler);
        _eventAggregator.GetEvent<StrategyUpdatedEvent>().Subscribe(UpdateTradesStrategyNameEventHandler);
        _eventAggregator.GetEvent<SelectedAccountUpdatedEvent>().Subscribe(SelectedAccountUpdatedEventHandler);
        _eventAggregator.GetEvent<StrategyDeletedEvent>().Subscribe(DeletedStrategyEventHandler);
    }

    private void DeletedStrategyEventHandler(StrategyDeletedDataBundle dataBundle)
    {
        var test = _tradeAccess.UpdateTradeStrategyName(dataBundle.FormerStrategyName, string.Empty);
        StrategyViewModel.DeleteStrategy(dataBundle.StrategyId);
        _ = LoadTrades();
    }
    
    private void SelectedAccountUpdatedEventHandler() => _ = LoadTrades();

    private void UpdateTradesStrategyNameEventHandler(StrategyUpdateDataBundle dataBundle)
    {
        _tradeAccess.UpdateTradeStrategyName(dataBundle.FormerStrategyName, dataBundle.NewStrategyName);
        _ = LoadTrades();
    }

    private void FireRequiredStrategyUsageDataEventHandler()
    {
        _eventAggregator.GetEvent<StrategyUsageDataRequiredEvent>().Publish(GetStrategyUsageData());
    }

    private void UpdateSelectedTradeHandler(object trade) => SelectedTrade = (Trade)trade;

    private void OpenTradeImagesHandler()
    {
        //I used dynamic typing here because this proyect can't see the implementation type of
        //the mainNavigationHelper menber just its interface type, thus I needed to avoid the
        //compile time check for the NavigateToTradeImagesCommand Property of the MainNavigationHelper
        //implementation
        dynamic navHelperRef = _mainNavigationHelper;
        navHelperRef.NavigateToTradeImagesCommand.Execute(null);
    }

    private void OpenTradeNotesHandler()
    {
        dynamic navHelperRef = _mainNavigationHelper;
        navHelperRef.NavigateToTradeNotesCommand.Execute(null);
    }

    private void OpenTradeMistakesHandler()
    {
        dynamic navHelperRef = _mainNavigationHelper;
        navHelperRef.NavigateToTradeMistakesCommand.Execute(null);
    }

    private void OpenTradeCostsHandler()
    {
        dynamic navHelperRef = _mainNavigationHelper;
        navHelperRef.NavigateToTradeCostsCommand.Execute(null);
    }

    private void OpenEditTradeWindowHandler()
    {
        dynamic navHelperRef = _mainNavigationHelper;
        navHelperRef.NavigateToEditTradeCommand.Execute(null);
    }

    private double[] CalculateReturnOnInvestment(TradeSide tradeSide,
                                                 double volume,
                                                 double openPrice,
                                                 double closePrice,
                                                 double swap,
                                                 double spread,
                                                 double commission,
                                                 double otherCosts,
                                                 int leverage)
    {
        double TradeCost = openPrice * volume / leverage;
        double profit = TradeCost / openPrice * (closePrice - openPrice) - swap - spread - commission - otherCosts;
        double ROI = profit / TradeCost * 100;

        if (tradeSide == 0)
        {
            ROI *= -1;
            profit *= -1;
        }

        return [profit, ROI];
    }

    private void CleanVMData()
    {
        OpenDateVM = new DateTime(0);
        ClosePriceVM = 0;
        SwapVM = 0;
        SpreadVM = 0;
        CommissionVM = 0;
        OtherCostsVM = 0;
        StopLossVM = 0;
        TakeProfitVM = 0;
        MistakesVM = "";
        NotesVM = "";
        LeverageVM = 0;
    }

    private void OnSelectedAccountChanged(object sender, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == nameof(AccountViewModel.SelectedAccount))
        {
            Trades.Clear();

            var tempDataReckords = _tradeAccess.QueryAccountTradesAsync(_accountViewModel.SelectedAccount.Id).Result;

            foreach (var i in tempDataReckords) Trades.Add(i);
        }
    }

    private List<StrategyUsageDataBundle> GetStrategyUsageData()
    {
        List<Trade> tradesWithStrategy = (from trade in Trades
                                          where trade.StrategyName != null
                                          select trade).ToList();

        return tradesWithStrategy
                .GroupBy(x => x.StrategyName)
                .Select(g => new StrategyUsageDataBundle
                {
                    StrategyName = g.Key!,
                    NumberOfUses = g.Count()
                }).ToList();
    }

    private List<StrategyPerformanceDataBundle> GetStrategyPerformanceData()
    {
        List<Trade> tradesWithStrategy = (from trade in Trades
                                          where trade.StrategyName != null
                                          select trade).ToList();

        return tradesWithStrategy
                .Select(x => new StrategyPerformanceDataBundle
                {
                    StrategyName = x.StrategyName,
                    IsLong = (int)x.Side,
                    IsOpen = (int)x.Side,
                    OpenPrice = x.OpenPrice,
                    ClosePrice = x.ClosePrice,
                    Swap = x.Swap,
                    Spread = x.Spread,
                    Commission = x.Commission,
                    OtherCosts = x.OtherCosts,
                }).ToList();
    }

    private bool IsWonTrade(Trade trade)
    {
        switch (trade.Side)
        {
            case TradeSide.Long:
                if (trade.Volume * trade.ClosePrice >= (trade.Volume * trade.OpenPrice)
                    + trade.Swap + trade.Spread + trade.Commission + trade.OtherCosts)
                    return true;

                else return false;
            case TradeSide.Short:
                if (trade.Volume * trade.ClosePrice <= (trade.Volume * trade.OpenPrice)
                    + trade.Swap + trade.Spread + trade.Commission + trade.OtherCosts)
                    return true;

                else return false;
        }

        return false;
    }

    private void RemoveLeftOverTrades(List<Trade> leftOverTrades)
    {
        foreach(var trade in leftOverTrades) Trades.Remove(trade);
    }

    private double CalculateNewAccountBalance(Trade addedTrade)
    {
        return addedTrade.Status == TradeStatus.Open ? 
            AccountViewModel.SelectedAccount.CurrentBalance - addedTrade.TradeCost
                                                            - addedTrade.Swap
                                                            - addedTrade.Spread
                                                            - addedTrade.Commission
                                                            - addedTrade.OtherCosts : // here the coalescing operator
            AccountViewModel.SelectedAccount.CurrentBalance - addedTrade.TradeCost
                                                            - addedTrade.Swap
                                                            - addedTrade.Spread
                                                            - addedTrade.Commission
                                                            - addedTrade.OtherCosts 
                                                            + addedTrade.Roi!.Value;
    }
}
