using CommunityToolkit.Mvvm.ComponentModel;
using MainModule.DataModel;
using System.Collections.ObjectModel;

namespace MainModule.ViewModels;

public partial class HomeViewModel : ObservableObject, IViewModel
{
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
}
