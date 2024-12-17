using MainModule.DataAccess;
using MainModule.Services;

namespace MainModule.Tests;

public class StrategyDataAccessTests
{
    private static readonly ConfigurationService dataAccessConfig = new();
    private static readonly StrategyAccess dataAccess = new(dataAccessConfig);

    [Fact]
    public void InsertStrategy_ShouldInsert()
    {
        int expected = 1;

        int actual = dataAccess.InsertStrategy(new()
        {
            Name = "testStrategy",
            Goal = "some goal",
            Intermediary = "Bing X",
            RiskRewardRatio = 10.32,
            MaxTradeRisk = 3213,
            DailyGoal = 1000,
            MaxDailyLoss = 1000
        });

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void UpdateStrategy_ShouldUpdate()
    {
        int expected = 1;
        int actual = dataAccess.UpdateStrategy(new()
        {
            Id = 1,
            Name = "testUpdate",
            Goal = "another goal",
            Intermediary = "Bing Y",
            RiskRewardRatio = 23.232,
            MaxTradeRisk = 12.34,
            DailyGoal = 100,
            MaxDailyLoss = 1212
        });

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void UpdateStrategyWonTrades_ShouldUpdateWins()
    {
        int expected = 1;
        int actual = dataAccess.UpdateStrategyWonTrades("testStrategy");

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void UpdateStrategyLostTrades_ShouldUpdateLosses()
    {
        int expected = 1;
        int actual = dataAccess.UpdateStrategyLostTrades("testStratey");

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void DeleteStrategy_ShouldDelete()
    {
        int expected = 1;
        int actual = dataAccess.DeleteStrategy(1);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void QueryStrategiesAsync_ShouldLoadStrategies()
    {
        int expected = 1;
        int actual = 0;

        if (dataAccess.QueryStrategiesAsync().IsCompletedSuccessfully) actual = expected;

        Assert.Equal(actual, expected);
    }
}
