
namespace API.Events
{
    public class StrategyUpdatedEvent : PubSubEvent<StrategyUpdateDataBundle>
    {
    }

    public class StrategyUpdateDataBundle
    {
        public string NewStrategyName { get; set; }

        public  string FormerStrategyName { get; set; }
    }
}



