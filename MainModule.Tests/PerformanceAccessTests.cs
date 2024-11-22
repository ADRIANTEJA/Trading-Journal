using MainModule.DataAccess;
using MainModule.DataModel;
using MainModule.Services;

namespace MainModule.Tests;

public class PerformanceAccessTests
{
    private static readonly ConfigurationService dataAccessConfig = new();
    private static readonly PerformanceAccess dataAccess = new(dataAccessConfig);

    [Fact]
    public void InsertDayPerformanceAsync_ShouldInsertDayPerformance()
    {
        int expected = 1;
        int actual = dataAccess.InsertDayPerformance(new Performance()
        {
            AccountId = 1,
            Date = 9999999999999999,
            ROI = 45.32,
            ROIPercentage = 4.12
        });

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void QueryDayPerformanceByAccountIdAsync_shouldLoadDayPerformanceByAccountId()
    {
        int expected = 1;

        int actual = 0;

        if (dataAccess.QueryDayPerformanceByAccountIdAsync(1).IsCompletedSuccessfully) actual = expected;

        Assert.Equal(expected, actual);
    }
}
