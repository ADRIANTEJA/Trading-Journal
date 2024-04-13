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
    private readonly IDataAccessConfiguration _dataAccessConfig;

    public AccountAccess(IDataAccessConfiguration dataAccessConfig)
    {
        _dataAccessConfig = dataAccessConfig; 
    }

    public int InsertAccount(Account account)
    {
        using var connection = new SQLiteConnection(_dataAccessConfig.GetConfiguration()["connection_string"]);
        return connection.Execute(@"INSERT INTO Account (name, initialBalance, currentBalance, isSelected)
                                    VALUES (@Name, @InitialBalance, @CurrentBalance, @IsSelected)", account);
    }

    public int UpdateAccount(int id, double currentBalance)
    {
        using var connection = new SQLiteConnection(_dataAccessConfig.GetConfiguration()["connection_string"]);
        return connection.Execute(@"UPDATE Account 
                                    SET currentBalance = @currentBalance
                                    WHERE id = @id", new { id , currentBalance });
    }

    public int DeleteAccount(int id)
    {
        using var connection = new SQLiteConnection(_dataAccessConfig.GetConfiguration()["connection_string"]);
        return connection.Execute(@"DELETE FROM Account 
                             WHERE id = @id", new { id });
    }

    public async Task<List<Account>> QueryAccountsAsync()
    {
        using var connection = new SQLiteConnection(_dataAccessConfig.GetConfiguration()["connection_string"]);
        var accounts = await connection.QueryAsync<Account>("SELECT * FROM Account");
        return accounts.ToList();
    }
}

