using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MainModule.DataAccess;
using MainModule.DataModel;
using System.Collections.ObjectModel;

namespace MainModule.ViewModels;

public partial class AccountViewModel : ObservableObject, IViewModel
{
    private AccountAccess _accountDataAccess;

    [ObservableProperty]
    private Account selectedAccount;

    [ObservableProperty]
    private ObservableCollection<Account> accounts;

    [ObservableProperty]
    private string nameVM;

    [ObservableProperty]
    private double initialBalanceVM;

    [ObservableProperty]
    private double currentlBalanceVM;

    [RelayCommand]
    public void AddAccount()
    {
        var newAccount = new Account
        {
            Name = NameVM,
            InitialBalance = InitialBalanceVM,
            CurrentBalance = CurrentlBalanceVM,
        };

        _accountDataAccess.InsertAccount(newAccount);
    }

    public AccountViewModel(AccountAccess accountDataAccess) 
    {
        _accountDataAccess = accountDataAccess;
        Accounts = new(_accountDataAccess.QueryAccountsAsync().Result);
        SelectedAccount = Accounts.First(ac => ac.IsSelected == 1);
    }
}
