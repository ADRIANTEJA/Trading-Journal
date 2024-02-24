
namespace MainModule.DataModel;

public class Trade
{
    /// <summary>
    /// the Trade primary and foreign key associated to an account
    /// </summary>
    public int AccountId { get; set; }
    /// <summary>
    /// the pair of assets traded
    /// </summary>
    public string PairTraded { get; set; }
    /// <summary>
    /// represents the date and hour the position(trade) was opened
    /// </summary>
    public DateTime OpenDate { get; set; }
    /// <summary>
    /// represents the date and hour the position(trade) was closed
    /// </summary>
    public DateTime? CloseDate { get; set; }
    /// <summary>
    /// represents on what side of the market the position(trade) was opened; long or short
    /// </summary>
    public string Side { get; set; }
    /// <summary>
    /// represents the volume of the asset operated e.g 100 usd
    /// </summary>
    public double Volume { get; set; }
    /// <summary>
    /// represents the current status of the position(trade); open or closed
    /// </summary>
    public string Status { get; set; }
    /// <summary>
    /// represents the price the traded symbol had when the position(trade) was opened
    /// </summary>
    public double OpenPrice { get; set; }
    /// <summary>
    /// represents the price the traded symbol had when the position(trade) was closed
    /// </summary>
    public double? ClosePrice { get; set; }
    /// <summary>
    /// represents the position(trade) initial cost per volume traded
    /// </summary>
    public double TradeCost { get; set; }
    /// <summary>
    /// represents the cost(or profit) of holding the position(trade) overnight,
    /// </summary>
    public double Swap { get; set; }
    /// <summary>
    /// represents the differencce between the buying price and the selling price
    /// </summary>
    public double Spread { get; set; }
    /// <summary>
    /// represents the commission charged by the intermediary for the service of managing the position(trade)
    /// </summary>
    public double Commission { get; set; }
    /// <summary>
    /// represents any other costs involved
    /// </summary>
    public double OtherCosts { get; set; }
    /// <summary>
    /// represents the maximun profit that can be taken in the position(trade)
    /// </summary>
    public double? TakeProfit { get; set;}
    /// <summary>
    /// represents the maximun loss allowed in the position(trade)
    /// </summary>
    public double? StopLoss { get; set; }
    /// <summary>
    /// represents the return on investment or the profit of the position(trade)
    /// </summary>
    public double? ROI { get; set; }
    /// <summary>
    /// represents the description or naming of any mistakes the users think to have
    /// committed in the position(trade)
    /// </summary>
    public string? Mistakes { get; set; }
    /// <summary>
    /// represents any note or description the user wants to add regarding its behavior
    /// while operating
    /// </summary>
    public string? Notes { get; set; }
}
