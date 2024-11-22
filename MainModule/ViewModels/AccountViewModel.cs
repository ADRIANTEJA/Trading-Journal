using API;
using API.Events;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MainModule.DataAccess;
using MainModule.DataModel;
using System.Collections.ObjectModel;
using System.Data.SQLite;

namespace MainModule.ViewModels;

public partial class AccountViewModel : ObservableObject, IViewModel
{
    public Func<double, string> TicksToDateConverter { get; } = 
        (double value) => new DateTime((long)value).ToString("yyyy-MM-dd");

    private readonly IEventAggregator _eventAggregator;

    private readonly INavigationHelper _mainNavigationHelper;

    public INavigationHelper MainNavigationHelper => _mainNavigationHelper;

    private readonly AccountAccess _accountAccess;

    private readonly PerformanceViewModel _performanceViewModel;

    public PerformanceViewModel PerformanceViewModel => _performanceViewModel;

    [ObservableProperty]
    private Account selectedAccount;

    public ObservableCollection<Account> Accounts { get; } = [];

    [ObservableProperty]
    private string nameVM;

    [ObservableProperty]
    private double initialBalanceVM;

    [RelayCommand]
    private void LoadAccounts()
    {
        Accounts.Clear();

        var tempDataReckords = _accountAccess.QueryAccountsAsync().Result;

        foreach (var account in tempDataReckords) Accounts.Add(account);

        SelectedAccount = Accounts.First(account => account.IsSelected == 1);
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

        try 
        {
            _accountAccess.InsertAccount(newAccount);
            Accounts.Add(newAccount);
            _eventAggregator.GetEvent<CreateAccountEvent>().Publish(true);
        }
        catch (SQLiteException) { _eventAggregator.GetEvent<CreateAccountEvent>().Publish(false); }   
    }

    [RelayCommand]
    private void FireLoadPerformanceEventCommand() => 
        _eventAggregator.GetEvent<CreatePerformanceEvent>().Publish(SelectedAccount.Id);

    public AccountViewModel(AccountAccess accountAccess,
                            PerformanceViewModel performanceViewModel,
                            INavigationHelper mainNavigationHelper,
                            IEventAggregator eventAggregator) 
    {
        _mainNavigationHelper = mainNavigationHelper;
        _accountAccess = accountAccess;
        _performanceViewModel = performanceViewModel;
        _eventAggregator = eventAggregator;
    }
}
