using CommandLine;

namespace TestConsole
{
    internal class Options
    {
        [Option('z', "zkconnect", Required = true, HelpText = "The Zookeeper connection string - e.g. 192.168.33.10:2181.")]
        public string ZkConnect { get; set; }
        [Option('t', "topic", Required = true, HelpText = "The name of the topic.")]
        public string Topic { get; set; }
        [Option('d', "duration", Required = false, HelpText = "The duration to execute for in seconds.", Default = 60)]
        public double Duration { get; set; }
        [Option('s', "sleep", Required = false, HelpText = "The duration to sleep between iterations in milliseconds.", Default = 100)]
        public int Sleep { get; set; }
        [Option('r', "report", Required = false, HelpText = "The amount of time between reports in seconds.", Default = 5)]
        public int Report { get; set; }
    }
}