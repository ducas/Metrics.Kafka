namespace Metrics.Kafka
{
    public class Counter
    {
        public Unit Unit { get; set; }
        public long Count { get; set; }
        public CounterItem[] Items { get; set; }
    }

    public class CounterItem
    {
        public long Count { get; set; }
        public string Name { get; set; }
        public double Percentage { get; set; }
    }
}