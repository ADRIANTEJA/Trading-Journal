using Dapper;
using MainModule.DataModel;
using MainModule.Services;
using System.Data.SQLite;

namespace MainModule.DataAccess;

public class AnalysisNoteAccess
{
    private readonly IConfigurationService _dataAccessConfig;

    public AnalysisNoteAccess(IConfigurationService dataAccessConfig)
    {
        _dataAccessConfig = dataAccessConfig;
    }

    public int InsertTradeNote(AnalysisNote analysisNote)
    {
        string command = @"INSERT INTO AnalysisNote (strategyId, title, text)
                               VALUES (@StrategyId, @Title, @Text)";

        using var connection = new SQLiteConnection(_dataAccessConfig.GetConfiguration()["connection_string"]);
        return connection.Execute(command, analysisNote);
    }

    public int DeleteTradeNote(int id)
    {
        string command = @"DELETE FROM AnalysisNote
                               WHERE id = @id";

        using var connection = new SQLiteConnection(_dataAccessConfig.GetConfiguration()["connection_string"]);
        return connection.Execute(command, new { id });
    }

    public async Task<List<AnalysisNote>> QueryStrategyAnalysisNotesAsync(int strategyId)
    {
        string command = @"SELECT * FROM AnalysisNote
                           WHERE strategyId = @strategyId";

        using var connection = new SQLiteConnection(_dataAccessConfig.GetConfiguration()["connection_string"]);
        var analysisNotes = await connection.QueryAsync<AnalysisNote>(command, new { strategyId });
        return analysisNotes.ToList();
    }
}
