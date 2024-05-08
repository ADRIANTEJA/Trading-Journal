
namespace MainModule.DataModel;

public class AnalysisNote
{
    /// <summary>
    /// the AnalysisNote primary key
    /// </summary>
    public int Id { get; set; }
    /// <summary>
    /// the AnalysisNote foreign key associated to a strategy
    /// </summary>
    public int StrategyId { get; set; }
    /// <summary>
    /// represents the title of the note that is shown in the analysis notes list
    /// </summary>
    public string Title { get; set; }
    /// <summary>
    /// represents the text contained in the note
    /// </summary>
    public string Text { get; set; }
}
