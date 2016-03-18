namespace Metrics.Kafka
{
    public class Timer
    {
        public Unit Unit { get; set; }
        public TimeUnit RateUnit { get; set; }
        public TimeUnit DurationUnit { get; set; }
        public long Count { get; set; }
        public long ActiveSessions { get; set; }
        public double MeanRate { get; set; }
        public double OneMinRate { get; set; }
        public double FiveMinRate { get; set; }
        public double FifteenMinRate { get; set; }
        public double Last { get; set; }
        public string LastUserValue { get; set; }
        public double Max { get; set; }
        public string MaxUserValue { get; set; }
        public double Mean { get; set; }
        public double Median { get; set; }
        public double Min { get; set; }
        public string MinUserValue { get; set; }
        public double Percentile75 { get; set; }
        public double Percentile95 { get; set; }
        public double Percentile98 { get; set; }
        public double Percentile99 { get; set; }
        public double Percentile999 { get; set; }
        public int SampleSize { get; set; }
        public double StdDev { get; set; }
    }
}