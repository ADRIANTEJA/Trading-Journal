using Dapper;
using MainModule.DataModel;
using MainModule.Services;
using System.Data.SQLite;

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
        string command = @"INSERT INTO Account (name, initialBalance, currentBalance, isSelected)
                           VALUES (@Name, @InitialBalance, @CurrentBalance, @IsSelected)";

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

    public int UpdateAccountIsSelectedStatus(int id, int isSelected)
    {
        string command = @"UPDATE Account
                           SET isSelected = @isSelected
                           WHERE id = @id";

        using var connection = new SQLiteConnection(_dataAccessConfig.GetConfiguration()["connection_string"]);
        return connection.Execute(command, new { id, isSelected });
    }

    public int UpdateAccountIsBankruptStatus(int id, int isBankrupt)
    {
        string command = @"UPDATE Account
                           SET isBankrupt = @isBankrupt
                           WHERE id = @id";
        using var connection = new SQLiteConnection(_dataAccessConfig.GetConfiguration()["connection_string"]);
        return connection.Execute(command, new { id, isBankrupt });
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

