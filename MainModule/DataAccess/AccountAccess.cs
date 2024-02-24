using Dapper;
using MainModule.DataModel;
using System.Configuration;
using System.Data;
using System.Data.SQLite;

namespace MainModule.DataAccess;
/// <summary>
/// Account entity data access class
/// </summary>
public class AccountAccess
{
    public static void InsertAccount(Account account)
    {
        using IDbConnection connection = new SQLiteConnection(LoadConnectionString("main_connection"));
        connection.Execute(@"INSERT INTO Account (name, initialBalance, actualBalance)
                             VALUES (@Name, @InitialBalance, @ActualBalance)", account);
    }

    public static void DeleteAccount(int id)
    {
        using IDbConnection connection = new SQLiteConnection(LoadConnectionString("main_connection"));
        connection.Execute("DELETE FROM Account WHERE id = @id", new { id });
    }

    public static async Task<List<Account>> QueryAccountsAsync()
    {
        using IDbConnection connection = new SQLiteConnection(LoadConnectionString("main_connection"));
        var accounts = await connection.QueryAsync<Account>("SELECT * FROM Account");
        return accounts.ToList();
    }

    private static string LoadConnectionString(string id)
    {
        return ConfigurationManager.ConnectionStrings[id].ConnectionString;
    }
}

