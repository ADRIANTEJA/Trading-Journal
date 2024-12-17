

namespace API.Events
{
    /// <summary>
    /// raised when the trades records are needed by other modules and
    /// used exclusively to deliver the loaded trades collection
    /// </summary>
    public class StrategyUsageDataRequiredEvent : PubSubEvent<List<StrategyUsageDataBundle>>
    {
    }

    public class StrategyUsageDataBundle
    {
        public string StrategyName { get; set; }
        public int NumberOfUses { get; set; }
    }
}


