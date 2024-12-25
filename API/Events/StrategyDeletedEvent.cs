
namespace API.Events
{
    public class StrategyDeletedEvent : PubSubEvent<StrategyDeletedDataBundle>
    {
    }

    public class StrategyDeletedDataBundle
    {
        public int StrategyId { get; set; }

        public string FormerStrategyName { get; set; }
    }
}


