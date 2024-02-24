using CommunityToolkit.Mvvm.ComponentModel;
using MainModule.DataModel;
using System.Collections.ObjectModel;

namespace MainModule.ViewModels;

public partial class AccountViewModel : ObservableObject, IViewModel
{
    [ObservableProperty]
    private ObservableCollection<Account> accounts;

    [ObservableProperty]
    private string name;

    [ObservableProperty]
    private double initialBalance;

    [ObservableProperty]
    private double actualBalance;
}
