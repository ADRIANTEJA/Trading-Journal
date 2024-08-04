using API;
using API.Events;
using CommunityToolkit.Mvvm.ComponentModel;
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

    private readonly TradeAccess _tradeAccess;

    public ObservableCollection<Trade> Trades { get; }

    private Trade selectedTrade;

    public Trade SelectedTrade { get; set; }

    [ObservableProperty]
    private int tradeId;

    [ObservableProperty]
    private string tradePairTraded;

    [ObservableProperty]
    private DateTime tradeOpenDate;

    [ObservableProperty]
    private DateTime tradeCloseDate;

    [ObservableProperty]
    private string tradeSide;

    [ObservableProperty]
    private double tradeVolume;

    [ObservableProperty]
    private string tradeStatus;

    [ObservableProperty]
    private double tradeOpenPrice;

    [ObservableProperty]
    private double tradeClosePrice;

    [ObservableProperty]
    private double tradeCost;

    [ObservableProperty]
    private double tradeSwap;

    [ObservableProperty]
    private double tradeSread;

    [ObservableProperty]
    private double tradeCommission;

    [ObservableProperty]
    private double otherTradeCosts;

    [ObservableProperty]
    private double tradeStopLoss;

    [ObservableProperty]
    private double tradeTakeProfit;

    [ObservableProperty]
    private double tradeROI;

    [ObservableProperty]
    private string tradeMistakes;

    [ObservableProperty]
    private string tradeNotes;

    public HomeViewModel(AccountViewModel accountViewModel, 
                         TradeAccess tradeAccess, 
                         IEventAggregator eventAggregator,
                         INavigationHelper mainNavigationHelper)
    {
        _accountViewModel = accountViewModel;
        _tradeAccess = tradeAccess;
        _mainNavigationHelper = mainNavigationHelper;
        _eventAggregator = eventAggregator;

        _eventAggregator.GetEvent<OnSelectedTradeItemChangedEvent>().Subscribe(UpdateSelectedTrade);
        _eventAggregator.GetEvent<OnLoadTradeImagesClickEvent>().Subscribe(OpenTradeImagesHandler);

        Trades = new(_tradeAccess.QueryAccountTradesAsync(_accountViewModel.SelectedAccount.Id).Result);
    }

    private void UpdateSelectedTrade(object trade)
    {
        SelectedTrade = (Trade)trade;
    }

    private void OpenTradeImagesHandler()
    {
        //I used dynamic typing here because this proyect can't see the implementation type of
        //the mainNavigationHelper menber just its interface type, thus I needed to avoid the
        //compile time check for the NavigateToTradeImagesCommand Property of the MainNavigationHelper
        //implementation
        dynamic navHelperRef = _mainNavigationHelper;
        navHelperRef.NavigateToTradeImagesCommand.Execute(null);
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
