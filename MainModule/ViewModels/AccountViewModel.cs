using API;
using API.Events;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using MainModule.DataAccess;
using MainModule.DataModel;
using Prism.Events;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.Windows;
using static MainModule.Common.Enums;

namespace MainModule.ViewModels;

public partial class AccountViewModel : ObservableObject, IViewModel
{
    private readonly IEventAggregator _eventAggregator;

    private readonly INavigationHelper _mainNavigationHelper;

    public INavigationHelper MainNavigationHelper => _mainNavigationHelper;

    private readonly AccountAccess _accountDataAccess;

    private DayPerformanceAccess _dayPerformanceDataAccess;

    private ROIFormat roiFormat = ROIFormat.Value;

    private PerfomanceTimeFrame performanceTimeFrame = PerfomanceTimeFrame.Daily;

    [ObservableProperty]
    private Account selectedAccount;

    public ObservableCollection<Account> Accounts { get; } = [];

    [ObservableProperty]
    private string nameVM;

    [ObservableProperty]
    private double initialBalanceVM;

    public ObservableCollection<DateTimePoint> AccountPerformanceValues { get; } = [];

    [ObservableProperty]
    private ISeries[] accountPerformance;

    [RelayCommand]
    public void AddAccount()
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
    public void LoadDailyPerformance()
    {
        if (SelectedAccount == null) return;

        var dayPerformance = _dayPerformanceDataAccess.QueryDayPerformanceByAccountIdAsync(SelectedAccount.Id).Result;

        switch (roiFormat)
        {
            case ROIFormat.Value:

                foreach (var i in dayPerformance)
                {
                    AccountPerformanceValues.Add(new(new(i.Date), i.ROI));
                }
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
        SelectedAccount = Accounts.First(ac => ac.IsSelected == 1);

        var seriesColor = new SolidColorPaint(new(47, 201, 123), 1);

        accountPerformance =
        [
            new LineSeries<DateTimePoint>
            {
                Values = AccountPerformanceValues,
                Fill = null,
                Stroke = seriesColor,
                GeometrySize = 5,
                GeometryFill = seriesColor,
                GeometryStroke = seriesColor
            }
        ];
    }
}
