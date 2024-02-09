
namespace MainModule.DataModel;

internal class TradeImage
{
    /// <summary>
    /// the TradeImage primary key
    /// </summary>
    public int Id { get; set; }
    /// <summary>
    /// the image foreign key associated to a trade
    /// </summary>
    public int TradeId { get; set; }
    /// <summary>
    /// represents the byte array that forms the image
    /// </summary>
    public byte[] Image { get; set; }
}
