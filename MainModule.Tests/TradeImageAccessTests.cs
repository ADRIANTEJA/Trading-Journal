using MainModule.DataAccess;
using MainModule.Services;

namespace MainModule.Tests;

public  class TradeImageAccessTests
{
    private static readonly ConfigurationService dataAccessConfig = new();
    private static readonly TradeImageAccess dataAccess = new(dataAccessConfig);

    [Fact]
    private void InsertTradeImage_ShouldInsert()
    {
        int expected = 1;
        int actual = dataAccess.InsertTradeImage(new()
        {
            Id = 1,
            TradeId = 1,
            Image = new byte[1000]
        });

        Assert.Equal(expected, actual);
    }

    [Fact]
    private void DeleteTradeImage_ShouldDelete()
    {
        int expected = 1;
        int actual = dataAccess.DeleteTradeImage(1);

        Assert.Equal(expected, actual);
    }

    [Fact]
    private void QueryTradeImagesAsync_ShouldLoadTradeImages()
    {
        int expected = 1;
        int actual = 0;

        if  (dataAccess.QueryTradeImagesAsync(1).IsCompletedSuccessfully) actual = expected;

        Assert.Equal(expected, actual);
    }
}
