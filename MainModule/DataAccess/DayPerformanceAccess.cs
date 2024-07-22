using Dapper;
using MainModule.DataModel;
using MainModule.Services;
using System.Data.SQLite;

namespace MainModule.DataAccess;

public class DayPerformanceAccess
{
    private readonly IConfigurationService _dataAccessConfig;

    public DayPerformanceAccess(IConfigurationService dataAccessConfig)
    {
        _dataAccessConfig = dataAccessConfig; 
    }

    public async Task<int> InsertDayPerformanceAsync(DayPerformance dayPerformance)
    {
        string command = @"INSERT INTO DayPerformance (accountId, date, roi, roiPercentage)
                           VALUES (@AccountId, @Date, @ROI, @ROIPercentage)";

        using var connection = new SQLiteConnection(_dataAccessConfig.GetConfiguration()["connection_string"]);
        return await connection.ExecuteAsync(command, dayPerformance);
    }

    public async Task<List<DayPerformance>> QueryDayPerformanceByAccountIdAsync(int accountId)
    {
        string commmand = @"SELECT * FROM DayPerformance
                            WHERE accountId = @accountId";

        using var connection = new SQLiteConnection(_dataAccessConfig.GetConfiguration()["connection_string"]);
        var dayPerformance = await connection.QueryAsync<DayPerformance>(commmand, new { accountId });
        return dayPerformance.ToList();
    }
}
