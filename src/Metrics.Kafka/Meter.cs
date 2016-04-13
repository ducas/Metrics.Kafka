namespace Metrics.Kafka
{
    public class Meter
    {
        public Unit Unit { get; set; }
        public TimeUnit TimeUnit { get; set; }
        public MeterItem Current { get; set; }
        public MeterItem[] Items { get; set; }
    }

    public class MeterItem
    {
        public long Count { get; set; }
        public double Percent { get; set; }
        public double MeanRate { get; set; }
        public double OneMinuteRate { get; set; }
        public double FiveMinuteRate { get; set; }
        public double FifteenMinuteRate { get; set; }
    }
}
