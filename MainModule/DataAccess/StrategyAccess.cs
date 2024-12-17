using Dapper;
using MainModule.DataModel;
using MainModule.Services;
using System.Data.SQLite;

namespace MainModule.DataAccess;

public class StrategyAccess
{
    private IConfigurationService _dataAccessConfig;

    public StrategyAccess(IConfigurationService dataAccessConfiguration)
    {
        _dataAccessConfig = dataAccessConfiguration;
    }

    public int InsertStrategy(Strategy strategy)
    {
        string command = @"INSERT INTO Strategy (name, goal, market, intermediary, riskRewardRatio, maxTradeRisk,
                        dailyGoal, maxDailyLoss)
                        VALUES (@Name, @Goal, @Market, @Intermediary, @RiskRewardRatio, @MaxTradeRisk, @DailyGoal,
                        @MaxDailyLoss)";

        using var connection = new SQLiteConnection(_dataAccessConfig.GetConfiguration()["connection_string"]);
        return connection.Execute(command, strategy);
    }

    public int UpdateStrategy(Strategy strategy)
    {
        string command = @"UPDATE Strategy 
                        SET name = @Name, goal = @Goal, market = @Market, intermediary = @Intermediary,
                        riskRewardRatio = @RiskRewardRatio, maxTradeRisk = @MaxTradeRisk, dailyGoal = @DailyGoal,
                        maxDailyLoss = @MaxDailyLoss
                        WHERE id = @Id";

        using var connection = new SQLiteConnection(_dataAccessConfig.GetConfiguration()["connection_string"]);
        return connection.Execute(command, strategy);
    }

    public int UpdateStrategyWonTrades(string name)
    {
        string command = @"UPDATE Strategy
                        SET wins = wins + 1
                        WHERE name = @name";

        using var connection = new SQLiteConnection(_dataAccessConfig.GetConfiguration()["connection_string"]);
        return connection.Execute(command, new { name });
    }

    public int UpdateStrategyLostTrades(string name)
    {
        string command = @"UPDATE Strategy
                           SET losses = losses + 1
                           WHERE name = @name";

        using var connection = new SQLiteConnection(_dataAccessConfig.GetConfiguration()["connection_string"]);
        return connection.Execute(command, new { name });
    }

    public int DeleteStrategy(int id)
    {
        var command = @"DELETE FROM Strategy
                        WHERE id = @id";

        using var connection = new SQLiteConnection(_dataAccessConfig.GetConfiguration()["connection_string"]);
        return connection.Execute(command, new { id });
    }

    public async Task<List<Strategy>> QueryStrategiesAsync()
    {
        var commnad = @"SELECT * FROM Strategy";

        using var connection = new SQLiteConnection(_dataAccessConfig.GetConfiguration()["connection_string"]);
        var strategies = await connection.QueryAsync<Strategy>(commnad);
        return strategies.ToList();
    }
}
