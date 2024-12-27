using API;
using API.Events;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using MainModule.DataAccess;
using MainModule.DataModel;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.Windows.Media;

namespace MainModule.ViewModels;

public partial class AccountViewModel : ObservableObject, IViewModel
{
    //delete if unused
    public Func<double, string> TicksToDateConverter { get; } = 
        (double value) => new DateTime((long)value).ToString("yyyy-MM-dd");

    public Func<ChartPoint, string> StrategyUseLabelFormatter { get; } =
        chartPoint => string.Format("{0} ({1:P})", chartPoint.Y, chartPoint.Participation);

    private readonly IEventAggregator _eventAggregator;

    public IEventAggregator EventAggregator => _eventAggregator;

    private readonly INavigationHelper _mainNavigationHelper;

    public INavigationHelper MainNavigationHelper => _mainNavigationHelper;

    private readonly AccountAccess _accountAccess;

    private readonly PerformanceViewModel _performanceViewModel;

    public PerformanceViewModel PerformanceViewModel => _performanceViewModel;

    [ObservableProperty]
    private Account selectedAccount = null;

    public ObservableCollection<Account> Accounts { get; } = [];

    [ObservableProperty]
    private string nameVM;

    [ObservableProperty]
    private double initialBalanceVM;

    public SeriesCollection StrategyUsageSeries { get; } = [];

    [RelayCommand]
    private void LoadAccounts()
    {
        Accounts.Clear();

        var tempDataReckords = _accountAccess.QueryAccountsAsync().Result;

        foreach (var account in tempDataReckords) Accounts.Add(account);

        if (Accounts.Count > 0) SelectedAccount = Accounts.First(account => account.IsSelected == 1);
    }

    [RelayCommand]
    private void AddAccount()
    {
        var newAccount = new Account
        {
            Name = NameVM,
            InitialBalance = InitialBalanceVM,
            CurrentBalance = InitialBalanceVM,
        };

        if (Accounts.Count == 0) newAccount.IsSelected = 1;

        try 
        {
            _accountAccess.InsertAccount(newAccount);
            LoadAccounts();
            _eventAggregator.GetEvent<CreateAccountEvent>().Publish(true);
        }
        catch (SQLiteException) { _eventAggregator.GetEvent<CreateAccountEvent>().Publish(false); }   
    }

    public AccountViewModel(AccountAccess accountAccess,
                            PerformanceViewModel performanceViewModel,
                            INavigationHelper mainNavigationHelper,
                            IEventAggregator eventAggregator) 
    {
        _mainNavigationHelper = mainNavigationHelper;
        _accountAccess = accountAccess;
        _performanceViewModel = performanceViewModel;
        _eventAggregator = eventAggregator;

        _eventAggregator.GetEvent<StrategyUsageDataRequiredEvent>().Subscribe(OnRequestedTradesRecievedHandler);
        _eventAggregator.GetEvent<SelectedAccountChangedEvent>().Subscribe(UpdateSelectedAccountHandler);
        _eventAggregator.GetEvent<AccountDeletedEvent>().Subscribe(DeleteAccountHandler);
    }

    private void DeleteAccountHandler(int accountId)
    {
        if (SelectedAccount.Id == accountId && Accounts.Count > 1)
        {
            _accountAccess.DeleteAccount(accountId);
            Accounts.Remove(Accounts.First(account => account.Id == accountId));
            _accountAccess.UpdateAccountIsSelectedStatus(Accounts.First().Id, 1);
        }
        else _accountAccess.DeleteAccount(accountId);

        LoadAccounts();
    }

    private void UpdateSelectedAccountHandler(int accountId)
    {
        if (_accountAccess.UpdateAccountIsSelectedStatus(SelectedAccount.Id, 0) == 1
            && _accountAccess.UpdateAccountIsSelectedStatus(accountId, 1) == 1)
        LoadAccounts();
    }

    private void OnRequestedTradesRecievedHandler(List<StrategyUsageDataBundle>? strategyUsageData)
    {
        if (strategyUsageData == null) return;

        StrategyUsageSeries.Clear();
        
        foreach (var usageData in strategyUsageData)
        {
            StrategyUsageSeries.Add(new PieSeries
            {
                Title = usageData.StrategyName,
                LabelPoint = StrategyUseLabelFormatter,
                DataLabels = true,
                FontSize = 16,
                Stroke = new SolidColorBrush { Color = Color.FromRgb(255, 255, 255) },
                StrokeThickness = 1.5,
                Values = new ChartValues<ObservableValue> { new(usageData.NumberOfUses) }
            });
        }
    }
}
