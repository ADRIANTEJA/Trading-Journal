using MainModule.DataAccess;
using MainModule.DataModel;
using MainModule.Services;

namespace MainModule.Tests;

public class PerformanceAccessTests
{
    private static readonly ConfigurationService dataAccessConfig = new();
    private static readonly PerformanceAccess dataAccess = new(dataAccessConfig);

    [Fact]
    public void InsertPerformanceAsync_ShouldInsertDayPerformance()
    {
        int expected = 1;
        int actual = dataAccess.InsertPerformance(new Performance()
        {
            AccountId = 1,
            Date = DateTime.Now.Ticks,
            ROI = 45.32,
            ROIPercentage = 4.12,
            Cost = 100
        });

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void DeletePerformanceByDate_shouldDeleteByDate()
    {
        var testPerformance = new Performance()
        {
            AccountId = 1,
            Date = new DateTime(2001, 6, 12, 4, 20, 0).Ticks,
            ROI = 45.32,
            ROIPercentage = 4.12,
            Cost = 100
        };

        dataAccess.InsertPerformance(testPerformance);

        int expected = 1;
        int actual = dataAccess.DeletePerformanceByDate(testPerformance.Date);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void QueryPerformanceByAccountIdAsync_shouldLoadDayPerformanceByAccountId()
    {
        int expected = 1;

        int actual = 0;

        if (dataAccess.QueryPerformanceByAccountIdAsync(1).IsCompletedSuccessfully) actual = expected;

        Assert.Equal(expected, actual);
    }
}
