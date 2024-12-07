using static MainModule.Common.Enums;

namespace MainModule.DataModel;

public class Performance
{
    /// <summary>
    /// the Performance primary and foreign key associated to an account
    /// </summary>
    public int AccountId { get; set; }
    /// <summary>
    /// represents the date of the day to measure account performance
    /// </summary>
    public long Date { get; set; }
    /// <summary>
    /// represents the return on investment in the day
    /// </summary>
    public double ROI { get; set; }
    /// <summary>
    /// represents the return on investment in the day with a percentage format
    /// </summary>
    public double ROIPercentage { get; set; }
    /// <summary>
    /// represents the total sum of costs of trades made in this instance Date
    /// </summary>
    public double Cost { get; set; }
    /// <summary>
    /// represents the time frame(daily, monthly or yearly) the performance
    /// record should be shown in the UI and its only set before the records are grouped
    /// and about to be displayed
    /// </summary>
    public PerfomanceTimeFrame? PerfomanceTimeFrame { get; set; }
}
