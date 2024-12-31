namespace MainModule.Common;

public static class Enums
{
    public enum ConnectionId
    {
        main
    }
    /// <summary>
    /// The format the ROI property should be displayed in the user interface
    /// </summary>
    public enum ROIFormat
    {
        Value,
        Percentage
    }

    public enum PerfomanceTimeFrame
    {
        Daily,
        Monthly,
        Yearly
    }

    public enum FilterKey
    {
        Win,
        Loss,
        Open,
        Short,
        Long,
        OpenDate,
        CloseDate,
        Symbol,
    }

    public enum AccountSelectionStatus
    {
        IsNotSelected,
        IsSelected
    }

    public enum AccountBankruptcyStatus
    {
        NotBankrupt,
        Bankrupt
    }

    public enum TradeStatus
    {
        Closed,
        Open
    }

    public enum TradeSide
    {
        Short,
        Long
    }
}
