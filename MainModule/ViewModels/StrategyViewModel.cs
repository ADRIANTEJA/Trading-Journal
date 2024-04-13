using CommunityToolkit.Mvvm.ComponentModel;
using MainModule.DataAccess;

namespace MainModule.ViewModels;

public partial class StrategyViewModel : ObservableObject, IViewModel
{
    private AccountAccess _accountAccess;

    [ObservableProperty]
    private int id;

    [ObservableProperty]
    private string name;

    [ObservableProperty]
    private string goal;

    [ObservableProperty]
    private string market;

    [ObservableProperty]
    private string intermediary;

    [ObservableProperty]
    private double riskRewardRatio;

    [ObservableProperty]
    private double maxTradeRisk;

    [ObservableProperty]
    private double dailyGoal;

    [ObservableProperty]
    private double maxDailyLoss;

    [ObservableProperty]
    private int wins;

    [ObservableProperty]
    private int losses;

    public StrategyViewModel()
    {
      
    }
}
