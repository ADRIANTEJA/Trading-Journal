using Dapper;
using MainModule.DataModel;
using MainModule.Services;
using System.Data.SQLite;

namespace MainModule.DataAccess;

public class SymbolAccess
{
    private readonly IConfigurationService _dataAccessConfig;

    public SymbolAccess(IConfigurationService dataAccessConfig)
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

        using var connection = new SQLiteConnection(_dataAccessConfig.GetConfiguration()["connection_string"]);
        var symbols = await connection.QueryAsync<Symbol>(command);
        return symbols.ToList();
    }

    public async Task<List<Symbol>> QuerySymbolsByAssetTypeAsync(string assetType)
    {
        string command = @"SELECT * FROM Symbol
                           WHERE assetType = @assetType";

        using var connection = new SQLiteConnection(_dataAccessConfig.GetConfiguration()["connection_string"]);
        var symbols = await connection.QueryAsync<Symbol>(command, new { assetType });
        return symbols.ToList();
    }
}
