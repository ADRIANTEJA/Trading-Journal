using MainModule.DataAccess;
using MainModule.DataModel;
using MainModule.Services;

namespace MainModule.Tests;
public class TradeDataAccessTests
{
    private static ConfigurationService dataAccessConfig = new();
    private static TradeAccess dataAccess = new(dataAccessConfig);

    [Fact]
    public void InsertTrade_ShouldInsert()
    {
        int expected = 1;
        int actual = 0;

        Trade newfullTrade = new()
        {
            AccountId = 1,
            PairTraded = "USD/EUR",
            OpenDate = DateTime.Now.Ticks,
            CloseDate = DateTime.Now.Ticks,
            IsLong = 1,
            Volume = 50.0,
            IsOpen = 1,
            OpenPrice = 1.3,
            ClosePrice = 1.5,
            TradeCost = 65.0,
            Swap = 0.3,
            Spread = 0.5,
            Commission = 0.1,
            OtherCosts = 0.0,
            TakeProfit = 1.5,
            StopLoss = 0.9,
            Roi = 4.0,
            RoiPercentage = 20,
            Mistakes = "some mistakes",
            Notes = "some notes"
        };

        Trade newPartialTrade = new()
        {
            AccountId = 1,
            PairTraded = "USD/EUR",
            OpenDate = DateTime.Now.Ticks,
            IsLong = 1,
            IsOpen = 1,
            OpenPrice = 1.3,
            TradeCost = 65.0,
            StopLoss = 0.96,
        };

        if (dataAccess.InsertTrade(newfullTrade) == 1 && dataAccess.InsertTrade(newPartialTrade) == 1) { actual = expected; }

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void UpdateTrade_ShouldUpdate()
    {
        int expected = 1;
        int actual = 0;

        Trade partiallyUpdatedTrade = new()
        {
            Id = 2,
            PairTraded = "USD/EUR",
            OpenDate = DateTime.Now.Ticks,
            IsLong = 1,
            IsOpen = 1,
            OpenPrice = 1.3,
            TradeCost = 65,
            StopLoss = 0.96,
        };

        Trade fullyUpdatedTrade = new()
        {
            Id = 1,
            PairTraded = "USD/EUR",
            OpenDate = DateTime.Now.Ticks,
            CloseDate = DateTime.Now.Ticks,
            IsLong = 1,
            Volume = 50,
            IsOpen = 1,
            OpenPrice = 1.3,
            ClosePrice = 1.5,
            TradeCost = 65,
            Swap = 0.03,
            Spread = 0.5,
            Commission = 0.1,
            OtherCosts = 0,
            TakeProfit = 1.5,
            StopLoss = 0.96,
            Roi = 4,
            RoiPercentage = 10,
            Mistakes = "some mistakes",
            Notes = "some notes"
        };

        if (dataAccess.UpdateTrade(partiallyUpdatedTrade) == 1
            && dataAccess.UpdateTrade(fullyUpdatedTrade) == 1) { actual = expected; }

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void DeleteTrade_ShouldDelete()
    {
        int expected = 1;
        int actual = dataAccess.DeleteTrade(1);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void QueryAccountTradesAsync_ShouldLoadTrades()
    {
        int expected = 1;
        int actual = 0;

        if (dataAccess.QueryAccountTradesAsync(1).IsCompletedSuccessfully) actual = expected;

        Assert.Equal(expected, actual); 
    }
}
