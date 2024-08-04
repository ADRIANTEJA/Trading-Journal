using Dapper;
using MainModule.DataModel;
using MainModule.Services;
using System.Data.SQLite;

namespace MainModule.DataAccess;

public class TradeImageAccess
{
    private readonly IConfigurationService _dataAccessConfig;

    public TradeImageAccess(IConfigurationService dataAccessConfig)
    {
        _dataAccessConfig = dataAccessConfig;
    }

    public int InsertTradeImage(TradeImage tradeImage)
    {
        var command = @"INSERT INTO TradeImage (tradeId, image)
                        VALUES (@TradeId, @Image)";

        using var connection = new SQLiteConnection(_dataAccessConfig.GetConfiguration()["connection_string"]);
        return connection.Execute(command, tradeImage);
    }

    public int DeleteTradeImage(int id)
    {
        var command = @"DELETE FROM TradeImage
                        WHERE id = @id";

        using var connection = new SQLiteConnection(_dataAccessConfig.GetConfiguration()["connection_string"]);
        return connection.Execute(command, new { id });
    }

    public async Task<List<TradeImage>>QueryTradeImagesAsync(int tradeId)
    {
        var command = @"SELECT * FROM TradeImage
                        WHERE tradeId = @tradeId";

        using var connection = new SQLiteConnection(_dataAccessConfig.GetConfiguration()["connection_string"]);
        var tradeImages =  await connection.QueryAsync<TradeImage>(command, new { tradeId });
        return tradeImages.ToList();
    }
}
