using Dapper;
using MainModule.DataModel;
using MainModule.Services;
using System.Data.SQLite;

namespace MainModule.DataAccess;

public class SymbolAcess
{
    private readonly IConfigurationService _dataAccessConfig;

    public SymbolAcess(IConfigurationService dataAccessConfig)
    {
        _dataAccessConfig = dataAccessConfig;
    }

    public int InsertSymbol(Symbol symbol)
    {
        string command = @"INSERT INTO Symbol (pair, assetType)
                        VALUES (@Pair, @AssetType)";

        using var connection = new SQLiteConnection(_dataAccessConfig.GetConfiguration()["connection_string"]);
        return connection.Execute(command, symbol);
    }

    public int DeleteSymbol(int id)
    {
        string command = @"DELETE FROM Symbol
                        WHERE id = @id";

        using var connection = new SQLiteConnection(_dataAccessConfig.GetConfiguration()["connection_string"]);
        return connection.Execute(command, new { id });
    }

    public async Task<List<Symbol>> QuerySymbolsAsync()
    {
        string command = @"SELECT * FROM Symbol";

        var connection = new SQLiteConnection(_dataAccessConfig.GetConfiguration()["connection_string"]);
        var symbols = await connection.QueryAsync<Symbol>(command);
        return symbols.ToList();
    }
}
