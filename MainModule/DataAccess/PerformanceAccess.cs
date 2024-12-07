using Dapper;
using MainModule.DataModel;
using MainModule.Services;
using System.Data.SQLite;

namespace MainModule.DataAccess;

public class PerformanceAccess
{
    private readonly IConfigurationService _dataAccessConfig;

    public PerformanceAccess(IConfigurationService dataAccessConfig)
    {
        _dataAccessConfig = dataAccessConfig; 
    }

    public int InsertDayPerformance(Performance performanceRecord)
    {
        string command = @"INSERT INTO Performance (accountId, date, roi, roiPercentage, cost)
                           VALUES (@AccountId, @Date, @ROI, @ROIPercentage, @Cost)";

        using var connection = new SQLiteConnection(_dataAccessConfig.GetConfiguration()["connection_string"]);
        return connection.Execute(command, performanceRecord);
    }

    public async Task<List<Performance>> QueryDayPerformanceByAccountIdAsync(int accountId)
    {
        string commmand = @"SELECT * FROM Performance
                            WHERE accountId = @accountId";

        using var connection = new SQLiteConnection(_dataAccessConfig.GetConfiguration()["connection_string"]);
        var performanceRecords = await connection.QueryAsync<Performance>(commmand, new { accountId });
        return performanceRecords.ToList();
    }
}
