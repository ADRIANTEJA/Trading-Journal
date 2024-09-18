using MainModule.DataAccess;
using MainModule.Services;

namespace MainModule.Tests;

public class AnalysisNotesDataAccessTests
{
    private static readonly ConfigurationService dataAccessConfig = new();
    private static readonly AnalysisNoteAccess dataAccess = new(dataAccessConfig);

    [Fact]
    public void InsertAnalysisNote_ShouldInsert()
    {
        int expected = 1; 
        int actual = dataAccess.InsertTradeNote(new()
        {
            StrategyId = 1,
            Title = "Test",
            Text = "Test Text",
        });

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void DeleteAnalysisNote_ShouldDelete()
    {
        int expected = 1; 
        int actual = dataAccess.DeleteTradeNote(1);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void QueryStrategyAnalysisNotes_ShouldLoadNotes()
    {
        int expected = 1;
        int actual = 0;

        if (dataAccess.QueryStrategyAnalysisNotesAsync(1).IsCompletedSuccessfully) actual = expected;
        
        Assert.Equal(expected, actual);
    }
}
