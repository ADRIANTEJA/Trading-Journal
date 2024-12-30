using Dapper;
using MainModule.DataModel;
using MainModule.Services;
using System.Data.SQLite;

namespace MainModule.DataAccess;

public class TradeAccess
{
    private readonly IConfigurationService _dataAccessConfig;

    public TradeAccess(IConfigurationService dataAccessConfig)
    {
        _dataAccessConfig = dataAccessConfig;
    }

    public int InsertTrade(Trade trade)
    {
        string command = @"INSERT INTO Trade (accountId, pairTraded, pairMarket, openDate, closeDate, isLong, volume, isOpen,
                           openPrice, closePrice, tradeCost, swap, spread, commission, otherCosts, takeProfit,
                           stopLoss, roi, roiPercentage, mistakes, notes, strategyName, leverage, accountBalance)
                           VALUES (@AccountId, @PairTraded, @PairMarket, @OpenDate, @CloseDate, @IsLong, @Volume, @IsOpen,
                           @OpenPrice, @ClosePrice, @TradeCost, @Swap, @Spread, @Commission, @OtherCosts, @TakeProfit,
                           @StopLoss, @Roi, @RoiPercentage, @Mistakes, @Notes, @StrategyName, @Leverage, @AccountBalance)";

        using var connection = new SQLiteConnection(_dataAccessConfig.GetConfiguration()["connection_string"]);
        return connection.Execute(command, trade);
    }

    public int UpdateTrade(Trade trade)
    {
        string command = @"UPDATE Trade 
                           SET openDate = @OpenDate, closeDate = @CloseDate, 
                           volume = @Volume, isOpen = @IsOpen, openPrice = @OpenPrice, closePrice = @ClosePrice,
                           tradeCost = @TradeCost, swap = @Swap, spread = @Spread, commission = @Commission,
                           otherCosts = @OtherCosts, takeProfit = @TakeProfit, stopLoss = @StopLoss,
                           roi = @Roi, roiPercentage = @RoiPercentage, mistakes = @Mistakes, notes = @Notes
                           WHERE id = @Id";

        using var connection = new SQLiteConnection(_dataAccessConfig.GetConfiguration()["connection_string"]);
        return connection.Execute(command, trade);
    }

    public int UpdateTradeStrategyName(string formerStrategyName, string newStrategyName)
    {
        string command = @"UPDATE Trade
                           SET strategyName = @newStrategyName
                           WHERE strategyName = @formerStrategyName";

        using var connection = new SQLiteConnection(_dataAccessConfig.GetConfiguration()["connection_string"]);
        return connection.Execute(command, new { formerStrategyName, newStrategyName });
    }

    public int DeleteTrade(int id)
    {
        string command = @"DELETE FROM Trade
                           WHERE id = @id";

        using var connection = new SQLiteConnection(_dataAccessConfig.GetConfiguration()["connection_string"]);
        return connection.Execute(command, new { id });
    }

    public async Task<List<Trade>> QueryAccountTradesAsync(int accountId)
    {
        string command = @"SELECT * FROM Trade
                           WHERE accountId = @accountId";

        using var connection = new SQLiteConnection(_dataAccessConfig.GetConfiguration()["connection_string"]);
        var trades = await connection.QueryAsync<Trade>(command, new { accountId });
        return trades.ToList();
    }
}
