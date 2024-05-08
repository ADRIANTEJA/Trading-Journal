using CommunityToolkit.Mvvm.ComponentModel;
using MainModule.DataAccess;
using MainModule.DataModel;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace MainModule.ViewModels;

public partial class HomeViewModel : ObservableObject, IViewModel
{
    private readonly AccountViewModel _accountViewModel;

    private readonly TradeAccess _tradeAccess;

    [ObservableProperty]
    private ObservableCollection<Trade> trades;

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

    public HomeViewModel(AccountViewModel accountViewModel, TradeAccess tradeAccess)
    {
        _accountViewModel = accountViewModel;
        _tradeAccess = tradeAccess;

        Trades = new(_tradeAccess.QueryAccountTradesAsync(_accountViewModel.SelectedAccount.Id).Result);
    }

    private void OnSelectedAccountChanged(object sender, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == nameof(_accountViewModel.SelectedAccount))
        {
            Trades = new(_tradeAccess.QueryAccountTradesAsync(_accountViewModel.SelectedAccount.Id).Result);
        }
    }
}
