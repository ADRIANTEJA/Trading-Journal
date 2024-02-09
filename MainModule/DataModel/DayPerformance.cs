
namespace MainModule.DataModel;

internal class DayPerformance
{
    /// <summary>
    /// the DayPerformance primary key and foreign key associated to an account
    /// </summary>
    public int AccountId { get; set; }
    /// <summary>
    /// represents the date of the day to measure account performance
    /// </summary>
    public DateTime Date { get; set; }
    /// <summary>
    /// represents the return on investment in the day
    /// </summary>
    public double ROI { get; set; }
    /// <summary>
    /// represents the return on investment in the day with a percentage format
    /// </summary>
    public double ROIPercentage { get; set; }
}
