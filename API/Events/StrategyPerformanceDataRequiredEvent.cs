

namespace API.Events
{
    public class StrategyPerformanceDataRequiredEvent : PubSubEvent<List<StrategyPerformanceDataBundle>> 
    {
    }

    public class StrategyPerformanceDataBundle
    {
        public string? StrategyName { get; set; }

        public int IsLong { get; set; }

        public int IsOpen { get; set; }

        public double OpenPrice { get; set; }

        public double? ClosePrice { get; set; }

        public double Volume { get; set; }

        public double? Swap { get; set; }

        public double? Spread { get; set; }

        public double? Commission { get; set; }

        public double? OtherCosts { get; set; }
    }
}
