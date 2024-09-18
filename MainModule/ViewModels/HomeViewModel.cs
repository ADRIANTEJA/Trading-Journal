using API;
using API.Events;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MainModule.DataAccess;
using MainModule.DataModel;
using Prism.Events;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace MainModule.ViewModels;

public partial class HomeViewModel : ObservableObject, IViewModel
{
    private readonly INavigationHelper _mainNavigationHelper;

    private readonly IEventAggregator _eventAggregator;

    private readonly AccountViewModel _accountViewModel;

    public AccountViewModel AccountViewModel => _accountViewModel;

    private readonly SymbolViewModel _symbolViewModel;

    public SymbolViewModel SymbolViewModel => _symbolViewModel;

    private readonly TradeAccess _tradeAccess;

    public ObservableCollection<Trade> Trades { get; } = [];

    private Trade selectedTrade;

    public Trade SelectedTrade { get; set; }

    [ObservableProperty]
    private int tradeId;

    [ObservableProperty]
    private string pairTradedVM;

    [ObservableProperty]
    private DateTime openDateVM;

    [ObservableProperty]
    private DateTime closeDateVM;

    [ObservableProperty]
    private string sideVM;

    [ObservableProperty]
    private double volumeVM;

    [ObservableProperty]
    private string statusVM;

    [ObservableProperty]
    private double openPriceVM;

    [ObservableProperty]
    private double closePriceVM;

    [ObservableProperty]
    private double costVM;

    [ObservableProperty]
    private double swapVM;

    [ObservableProperty]
    private double spreadVM;

    [ObservableProperty]
    private double commissionVM;

    [ObservableProperty]
    private double otherCostsVM;

    [ObservableProperty]
    private double stopLossVM;

    [ObservableProperty]
    private double takeProfitVM;

    [ObservableProperty]
    private double tradeROIVM;

    [ObservableProperty]
    private string mistakesVM;

    [ObservableProperty]
    private string notesVM;

    [RelayCommand]
    private void LoadTrades()
    {
        Trades.Clear();

        var tempDataReckords =
            _tradeAccess.QueryAccountTradesAsync(_accountViewModel.SelectedAccount.Id).Result;

        foreach (var trade in tempDataReckords) Trades.Add(trade);
    }

    [RelayCommand]
    private void AddTrade(bool isTradeOpen)
    {
        int isOpen = 1;
        
    }

    public HomeViewModel(AccountViewModel accountViewModel,
                         SymbolViewModel symbolViewModel,        
                         TradeAccess tradeAccess, 
                         IEventAggregator eventAggregator,
                         INavigationHelper mainNavigationHelper)
    {
        _accountViewModel = accountViewModel;
        _symbolViewModel = symbolViewModel;
        _tradeAccess = tradeAccess;
        _mainNavigationHelper = mainNavigationHelper;
        _eventAggregator = eventAggregator;

        _eventAggregator.GetEvent<OnSelectedTradeItemChangedEvent>().Subscribe(UpdateSelectedTradeHandler);
        _eventAggregator.GetEvent<OnLoadTradeImagesClickEvent>().Subscribe(OpenTradeImagesHandler);
        _eventAggregator.GetEvent<OnLoadTradeNotesClickEvent>().Subscribe(OpenTradeNotesHandler);
        _eventAggregator.GetEvent<OnLoadTradeMistakesClickEvent>().Subscribe(OpenTradeMistakesHandler);
        _eventAggregator.GetEvent<OnLoadTradeCostsClickEvent>().Subscribe(OpenTradeCostsHandler);
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

    private void OnSelectedAccountChanged(object sender, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == nameof(AccountViewModel.SelectedAccount))
        {
            Trades.Clear();

            var tempDataReckords = _tradeAccess.QueryAccountTradesAsync(_accountViewModel.SelectedAccount.Id).Result;

            foreach (var i in tempDataReckords) Trades.Add(i);
        }
    }
}
