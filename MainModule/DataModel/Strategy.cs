
namespace MainModule.DataModel;

public class Strategy
{
    /// <summary>
    /// the Strategy primary key
    /// </summary>
    public int Id { get; set; }
    /// <summary>
    /// represents the strategy name for its identification in the UI 
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// represents the main porpuse to achieve by using the strategy
    /// </summary>
    public string? Goal { get; set; }
    /// <summary>
    /// symbol pair's id to be the strategy's main focus on investment,
    /// it may be none
    /// </summary>
    public string Market { get; set; }
    /// <summary>
    /// represents the broker or exchange the user its using to operate
    /// </summary>
    public string? Intermediary { get; set;}
    /// <summary>
    /// represents the % of the beneficts over the investment expected in every trade
    /// </summary>
    public double RiskRewardRatio { get; set; }
    /// <summary>
    /// represents the maximun % of capital risked in individual trades
    /// </summary>
    public double MaxTradeRisk { get; set;}
    /// <summary>
    /// represents the % expected to be earned daily usign the strategy
    /// </summary>
    public double DailyGoal { get; set; }
    /// <summary>
    /// represents the maximun % allowed to be loss in a day
    /// whitout violating the strategy
    /// </summary>
    public double MaxDailyLoss { get; set; }
    /// <summary>
    /// represents the nomber of won trades operating with the strategy 
    /// </summary>
    public int Wins { get; set;}
    /// <summary>
    /// represents the nomber of lost trades operating with the strategy
    /// </summary>
    public double Losses { get;}
}