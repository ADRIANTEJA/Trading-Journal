using API;
using API.Events;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LiveCharts;
using LiveCharts.Defaults;
using MainModule.DataAccess;
using MainModule.DataModel;
using Prism.Events;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using static MainModule.Common.Enums;

namespace MainModule.ViewModels;

public partial class AccountViewModel : ObservableObject, IViewModel
{
    public Func<double, string> TicksToDateConverter { get; } = 
        (double value) => new DateTime((long)value).ToString("yyyy-MM-dd");

    private readonly IEventAggregator _eventAggregator;

    private readonly INavigationHelper _mainNavigationHelper;

    public INavigationHelper MainNavigationHelper => _mainNavigationHelper;

    private readonly AccountAccess _accountDataAccess;

    private DayPerformanceAccess _dayPerformanceDataAccess;

    private ROIFormat roiFormat = ROIFormat.Value;

    private PerfomanceTimeFrame performanceTimeFrame = PerfomanceTimeFrame.Daily;

    [ObservableProperty]
    private Account selectedAccount;

    public ObservableCollection<Account> Accounts { get; }

    [ObservableProperty]
    private string nameVM;

    [ObservableProperty]
    private double initialBalanceVM;

    [ObservableProperty]
    public ChartValues<ObservablePoint> accountPerformance = [];

    [RelayCommand]
    private void AddAccount()
    {
        var newAccount = new Account
        {
            Name = NameVM,
            InitialBalance = InitialBalanceVM,
            CurrentBalance = InitialBalanceVM,
        };

        try 
        {
            _accountDataAccess.InsertAccount(newAccount);
            Accounts.Add(newAccount);
            _eventAggregator.GetEvent<OnCreateAccountEvent>().Publish(true);
        }
        catch (SQLiteException) { _eventAggregator.GetEvent<OnCreateAccountEvent>().Publish(false); }   
    }

    [RelayCommand]
    private void LoadDailyPerformance()
    {
        if (SelectedAccount == null) return;

        AccountPerformance.Clear();

        var tempReckordsList = _dayPerformanceDataAccess.QueryDayPerformanceByAccountIdAsync(SelectedAccount.Id).Result;

        List<DayPerformance> performance = [];

        switch (performanceTimeFrame)
        {
            case PerfomanceTimeFrame.Daily:

                performance = tempReckordsList;
                break;
            case PerfomanceTimeFrame.Monthly:

                performance = tempReckordsList
                    .Select(x => new { Date = new DateTime(x.Date), x.ROI, x.ROIPercentage })
                    .GroupBy(x => new { x.Date.Month, x.Date.Year })
                    .Select(g => new DayPerformance
                    {
                        Date = new DateTime(g.Key.Year, g.Key.Month, 1).Ticks,
                        ROI = g.Sum(x => x.ROI),
                        ROIPercentage = g.Sum(x => x.ROIPercentage) //Fix the ROI percentage calculation formula
                    }).ToList();
                break;
        }

        switch (roiFormat)
        {
            case ROIFormat.Value:

                foreach (var i in performance) AccountPerformance.Add(new(DateTime.Now.Ticks, i.ROI)); 
                break;
            case ROIFormat.Percentage:

                foreach (var i in performance) AccountPerformance.Add(new(DateTime.Now.Ticks, i.ROIPercentage));
                break;
        }
    }

    public AccountViewModel(AccountAccess accountDataAccess,
                            DayPerformanceAccess dayPerformanceDataAccess,
                            INavigationHelper mainNavigationHelper,
                            IEventAggregator eventAggregator) 
    {
        _mainNavigationHelper = mainNavigationHelper;
        _accountDataAccess = accountDataAccess;
        _dayPerformanceDataAccess = dayPerformanceDataAccess;
        _eventAggregator = eventAggregator;

        Accounts = new(_accountDataAccess.QueryAccountsAsync().Result);
        SelectedAccount = Accounts.First(account => account.IsSelected == 1);
    }
}
