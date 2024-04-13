
namespace MainModule.DataModel;

public class Symbol
{
    /// <summary>
    /// the Symbol primary key
    /// </summary>
    public int Id { get; }
    /// <summary>
    /// represents the asset pair used in trades and defined in strategies
    /// </summary>
    public string Pair { get; set; }
    /// <summary>
    /// represents the asset type of the buyed(long) or selled(short) asset
    /// </summary>
    public string AssetType { get; set;}
}
