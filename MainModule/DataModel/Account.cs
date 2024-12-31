using static MainModule.Common.Enums;

namespace MainModule.DataModel;

public class Account
{
    /// <summary>
    /// the Account primary key
    /// </summary>
    public int Id { get; set; }
    /// <summary>
    /// represents the account name for its identification in the UI
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// represents the account initial balance when created by the user
    /// </summary>
    public double InitialBalance { get; set; }
    /// <summary>
    /// represents the account actual balance
    /// </summary>
    public double CurrentBalance { get; set;}
    /// <summary>
    /// represents whether or not the account is selected as the current analysis subject
    /// </summary>
    public AccountSelectionStatus SelectionStatus { get; set; }
    /// <summary>
    /// represents whether or not the account's balance has reached zero
    /// </summary>
    public AccountBankruptcyStatus BankruptcyStatus { get; set; }
}
