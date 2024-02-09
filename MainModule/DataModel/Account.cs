
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
    public double ActualBalance { get; set;}
}
