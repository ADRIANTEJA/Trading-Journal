using API;
using API.Events;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MainModule.DataAccess;
using MainModule.DataModel;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SQLite;

namespace MainModule.ViewModels;

public partial class HomeViewModel : ObservableObject, IViewModel
{
    //Remenber to delete if unused
    public Func<double, string> TicksToDateConverter { get; } =
        (double value) => new DateTime((long)value).ToString("yyyy-MM-dd");

    private readonly INavigationHelper _mainNavigationHelper;

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
    private void LoadTrades()
    {
        Trades.Clear();

        if (_accountViewModel.SelectedAccount != null)
        {
            var tempDataReckords =
                _tradeAccess.QueryAccountTradesAsync(_accountViewModel.SelectedAccount.Id).Result;

            foreach (var trade in tempDataReckords) Trades.Add(trade);

            _eventAggregator.GetEvent<StrategyPerformanceDataRequiredEvent>().Publish(GetStrategyPerformanceData());
        }
    }

    [RelayCommand]
    private void AddOpenTrade(int isLongTrade)
    {
        string strategyName;

        if (StrategyViewModel.SelectedStrategyVM == null
            || StrategyViewModel.SelectedStrategyVM.Name.Length < 0) strategyName = null;
        else strategyName = StrategyViewModel.SelectedStrategyVM.Name;

        var openTrade = new Trade
        {
            AccountId = _accountViewModel.SelectedAccount.Id,
            PairTraded = SymbolViewModel.SelectedSymbolVM.Pair,
            PairMarket = SymbolViewModel.SelectedSymbolVM.AssetType,
            OpenDate = OpenDateVM.Ticks,
            IsLong = isLongTrade,
            Volume = VolumeVM,
            IsOpen = 1,
            OpenPrice = OpenPriceVM,
            TradeCost = OpenPriceVM * VolumeVM,
            Swap = SwapVM,
            Spread = SpreadVM,
            Commission = CommissionVM,
            OtherCosts = OtherCostsVM,
            TakeProfit = TakeProfitVM,
            StopLoss = StopLossVM,
            Mistakes = MistakesVM,
            Notes = NotesVM,
            StrategyName = strategyName,
            Leverage = LeverageVM
        };

        try
        {
            _tradeAccess.InsertTrade(openTrade);
            LoadTrades();
            _eventAggregator.GetEvent<CreateTradeEvent>().Publish(true);
            CleanVMData();
        }
        catch (SQLiteException) { _eventAggregator.GetEvent<CreateTradeEvent>().Publish(false); }
    }

    [RelayCommand]
    private void AddClosedTrade(int isLongTrade)
    {
        string strategyName;

        if (StrategyViewModel.SelectedStrategyVM == null
            || StrategyViewModel.SelectedStrategyVM.Name.Length < 0) strategyName = null;
        else strategyName = StrategyViewModel.SelectedStrategyVM.Name;

        var closedTrade = new Trade()
        {
            AccountId = _accountViewModel.SelectedAccount.Id,
            PairTraded = SymbolViewModel.SelectedSymbolVM.Pair,
            PairMarket = SymbolViewModel.SelectedSymbolVM.AssetType,
            OpenDate = CloseDateVM.Ticks,
            CloseDate = CloseDateVM.Ticks,
            IsLong = isLongTrade,
            Volume = VolumeVM,
            IsOpen = 0,
            OpenPrice = OpenPriceVM,
            ClosePrice = ClosePriceVM,
            TradeCost = VolumeVM * OpenPriceVM,
            Swap = SwapVM,
            Spread = SpreadVM,
            Commission = CommissionVM,
            OtherCosts = OtherCostsVM,
            TakeProfit = TakeProfitVM,
            StopLoss = StopLossVM,
            Mistakes = MistakesVM,
            Notes = NotesVM,
            Roi = CalculateReturnOnInvestment(isLongTrade,
                                              VolumeVM,
                                              OpenPriceVM,
                                              ClosePriceVM,
                                              SwapVM,
                                              SpreadVM,
                                              CommissionVM,
                                              OtherCostsVM)[0],
            RoiPercentage = CalculateReturnOnInvestment(isLongTrade,
                                              VolumeVM,
                                              OpenPriceVM,
                                              ClosePriceVM,
                                              SwapVM,
                                              SpreadVM,
                                              CommissionVM,
                                              OtherCostsVM)[1],
            StrategyName = strategyName,
            Leverage = LeverageVM,
        };

        try
        {
            _tradeAccess.InsertTrade(closedTrade);
            LoadTrades();

            if (closedTrade.StrategyName != null && IsWonTrade(closedTrade))
                _strategyViewModel.UpdateStrategyWonTradesCommand.Execute(closedTrade.StrategyName);
            else if (closedTrade.StrategyName != null)
                _strategyViewModel.UpdateStrategyLostTradesCommand.Execute(closedTrade.StrategyName);

            _eventAggregator.GetEvent<CreateTradeEvent>().Publish(true);

            var performance = new Performance
            {
                AccountId = AccountViewModel.SelectedAccount.Id,
                Date = CloseDateVM.Ticks,
                ROI = CalculateReturnOnInvestment(isLongTrade,
                                                  VolumeVM,
                                                  OpenPriceVM,
                                                  ClosePriceVM,
                                                  SwapVM,
                                                  SpreadVM,
                                                  CommissionVM,
                                                  OtherCostsVM)[0],
                Cost = (VolumeVM * OpenPriceVM) + SwapVM + SpreadVM + CommissionVM + OtherCostsVM
            };

            _performanceViewModel.AddAccountPerformanceRecord(performance);
            _performanceViewModel.LoadDailyPerformance(_accountViewModel.SelectedAccount.Id);
            //_eventAggregator.GetEvent<StrategyUsageDataRequiredEvent>().Publish(GetStrategyUsageData()); 
            //delete if unused

            CleanVMData();
        }
        catch (SQLiteException) { _eventAggregator.GetEvent<CreateTradeEvent>().Publish(false); }
    }

    [RelayCommand]
    private void UpdateTrade(Trade updatedTrade)
    {
        if (updatedTrade.IsOpen == 0)
        {
            _performanceViewModel.DeletePerformanceByDate((long)updatedTrade.CloseDate!);

            var updatedPerformance = new Performance
            {
                AccountId = _accountViewModel.SelectedAccount.Id,
                Date = (long)updatedTrade.CloseDate,
                ROI = CalculateReturnOnInvestment(updatedTrade.IsLong,
                                                  updatedTrade.Volume,
                                                  updatedTrade.OpenPrice,
                                                  (double)updatedTrade.ClosePrice!,
                                                  updatedTrade.Swap,
                                                  updatedTrade.Spread,
                                                  updatedTrade.Commission,
                                                  updatedTrade.OtherCosts)[0],
                Cost = (updatedTrade.Volume * updatedTrade.OpenPrice) 
                + updatedTrade.Swap + updatedTrade.Spread + updatedTrade.Commission + updatedTrade.OtherCosts
            };

            _performanceViewModel.AddAccountPerformanceRecord(updatedPerformance);
            _performanceViewModel.LoadDailyPerformance(_accountViewModel.SelectedAccount.Id);

            _tradeAccess.UpdateTrade(updatedTrade);
            LoadTrades();
        }

        _tradeAccess.UpdateTrade(updatedTrade);
        LoadTrades();
    }

    [RelayCommand]
    private void DeleteTrade(int id)
    {
        if (_tradeAccess.DeleteTrade(id) == 1)
        {
            var deletedTrade = Trades.Where(x => x.Id == id).FirstOrDefault();
            
            Trades.Remove(deletedTrade!);
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
        LoadTrades();
    }
    
    private void SelectedAccountUpdatedEventHandler() => LoadTrades();

    private void UpdateTradesStrategyNameEventHandler(StrategyUpdateDataBundle dataBundle)
    {
        _tradeAccess.UpdateTradeStrategyName(dataBundle.FormerStrategyName, dataBundle.NewStrategyName);
        LoadTrades();
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

    private double[] CalculateReturnOnInvestment(int isLongTrade,
                                                 double volume,
                                                 double openPrice,
                                                 double closePrice,
                                                 double swap,
                                                 double spread,
                                                 double commission,
                                                 double otherCosts)
    {
        int leverage;

        if (LeverageVM == 0) leverage = 1;
        else leverage = LeverageVM;

        double TradeCost = openPrice * volume / leverage;
        double profit = TradeCost / openPrice * (closePrice - openPrice) - swap - spread - commission - otherCosts;
        double ROI = profit / TradeCost * 100;

        if (isLongTrade == 0)
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
                    IsLong = x.IsLong,
                    IsOpen = x.IsOpen,
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
        switch (trade.IsLong)
        {
            case 1:
                if (trade.Volume * trade.ClosePrice >= (trade.Volume * trade.OpenPrice)
                    + trade.Swap + trade.Spread + trade.Commission + trade.OtherCosts)
                    return true;

                else return false;
            case 2:
                if (trade.Volume * trade.ClosePrice <= (trade.Volume * trade.OpenPrice)
                    + trade.Swap + trade.Spread + trade.Commission + trade.OtherCosts)
                    return true;

                else return false;
        }

        return false;
    }
}
