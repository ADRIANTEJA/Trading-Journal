
namespace MainModule.DataModel;

public class Symbol
{
    /// <summary>
    /// the Symbol primary key
    /// </summary>
    public int Id { get; }
    /// <summary>
    /// represents the asset pairVM used in trades and defined in strategies
    /// </summary>
    public string Pair { get; set; }
    /// <summary>
    /// represents the asset type of the buyed(long) or selled(short) asset. Starts with uppercase
    /// </summary>
    public string AssetType { get; set;}
}
