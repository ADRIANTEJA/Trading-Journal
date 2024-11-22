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

    public int InsertDayPerformance(Performance dayPerformance)
    {
        string command = @"INSERT INTO Performance (accountId, date, roi, roiPercentage)
                           VALUES (@AccountId, @Date, @ROI, @ROIPercentage)";

        using var connection = new SQLiteConnection(_dataAccessConfig.GetConfiguration()["connection_string"]);
        return connection.Execute(command, dayPerformance);
    }

    public async Task<List<Performance>> QueryDayPerformanceByAccountIdAsync(int accountId)
    {
        string commmand = @"SELECT * FROM Performance
                            WHERE accountId = @accountId";

        using var connection = new SQLiteConnection(_dataAccessConfig.GetConfiguration()["connection_string"]);
        var dayPerformance = await connection.QueryAsync<Performance>(commmand, new { accountId });
        return dayPerformance.ToList();
    }
}
