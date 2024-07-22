using MainModule.DataAccess;
using MainModule.DataModel;
using MainModule.Services;

namespace MainModule.Tests;

public class DayPerformanceAccessTests
{
    private static readonly ConfigurationService dataAccessConfig = new();
    private static readonly DayPerformanceAccess dataAccess = new(dataAccessConfig);

    [Fact]
    public void InsertDayPerformanceAsync_ShouldInsertDayPerformance()
    {
        int expected = 1;

        int actual = 0;

        var dayPerformance = new DayPerformance()
        {
            AccountId = 1,
            Date = "10/02/24",
            ROI = 45.32,
            ROIPercentage = 4.12
        };

        if (dataAccess.InsertDayPerformanceAsync(dayPerformance).IsCompletedSuccessfully) actual = 1;

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void QueryDayPerformanceByAccountIdAsync_shouldLoadDayPerformanceByAccountId()
    {
        int expected = 1;

        int actual = 0;

        if (dataAccess.QueryDayPerformanceByAccountIdAsync(1).IsCompletedSuccessfully) actual = 1;

        Assert.Equal(expected, actual);
    }
}
