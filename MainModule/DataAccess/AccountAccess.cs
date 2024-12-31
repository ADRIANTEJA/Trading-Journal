using Dapper;
using MainModule.DataModel;
using MainModule.Services;
using System.Data.SQLite;
using static MainModule.Common.Enums;

namespace MainModule.DataAccess;
/// <summary>
/// Account entity data access class
/// </summary>
public class AccountAccess
{
    private readonly IConfigurationService _dataAccessConfig;

    public AccountAccess(IConfigurationService dataAccessConfig)
    {
        _dataAccessConfig = dataAccessConfig; 
    }

    public int InsertAccount(Account account)
    {
        string command = @"INSERT INTO Account (name, initialBalance, currentBalance, selectionStatus)
                           VALUES (@Name, @InitialBalance, @CurrentBalance, @SelectionStatus)";

        using var connection = new SQLiteConnection(_dataAccessConfig.GetConfiguration()["connection_string"]);
        return connection.Execute(command, account);
    }

    public int UpdateAccountBalance(int id, double currentBalance)
    {
        string command = @"UPDATE Account 
                           SET currentBalance = @currentBalance
                           WHERE id = @id";

        using var connection = new SQLiteConnection(_dataAccessConfig.GetConfiguration()["connection_string"]);
        return connection.Execute(command, new { id , currentBalance });
    }

    public int UpdateAccountName(int id, string newName)
    {
        string command = @"UPDATE Account
                           SET name = @newName
                           WHERE id = @id";

        using var connection = new SQLiteConnection(_dataAccessConfig.GetConfiguration()["connection_string"]);
        return connection.Execute(command, new { id, newName });
    }

    public int UpdateAccountIsSelectedStatus(int id, AccountSelectionStatus selectionStatus)
    {
        string command = @"UPDATE Account
                           SET selectionStatus = @selectionStatus
                           WHERE id = @id";

        using var connection = new SQLiteConnection(_dataAccessConfig.GetConfiguration()["connection_string"]);
        return connection.Execute(command, new { id, selectionStatus });
    }

    public int UpdateAccountBankruptcyStatus(int id, AccountBankruptcyStatus bankruptcyStatus)
    {
        string command = @"UPDATE Account
                           SET bankruptcyStatus = @bankruptcyStatus
                           WHERE id = @id";
        using var connection = new SQLiteConnection(_dataAccessConfig.GetConfiguration()["connection_string"]);
        return connection.Execute(command, new { id, bankruptcyStatus });
    }

    public int DeleteAccount(int id)
    {
        string command = @"DELETE FROM Account 
                           WHERE id = @id";

        using var connection = new SQLiteConnection(_dataAccessConfig.GetConfiguration()["connection_string"]);
        return connection.Execute(command, new { id });
    }

    public async Task<List<Account>> QueryAccountsAsync()
    {
        string command = "SELECT * FROM Account";

        using var connection = new SQLiteConnection(_dataAccessConfig.GetConfiguration()["connection_string"]);
        var accounts = await connection.QueryAsync<Account>(command);
        return accounts.ToList();
    }
}

