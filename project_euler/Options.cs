using CommandLine;

namespace project_euler
{
    partial class Program
    {
        public class Options
        {
            [Option('f', "file", Required = false, Default = null, HelpText = "Path to file containing configuration options")]
            public string ConfigurationFileName { get; set; }

            [Option('b', "brokers", Required = false, Default = null, HelpText = "Comma seperated list of Kafka brokers")]
            public string Brokers { get; set; }

            [Option('j', "job-topic", Required = false, Default = null, HelpText = "Name of the Jobs topic to subscribe to")]
            public string JobTopic { get; set; }

            [Option('a', "answer-topic", Required = false, Default = null, HelpText = "Name of the answer topic to publish to")]
            public string AnswerTopic { get; set; }

            [Option('c', "ca-file", Required = false, Default = null, HelpText = "Path to ssl ca file")]
            public string CaFile { get; set; }

            [Option('k', "key-file", Required = false, Default = null, HelpText = "Path to ssl key file")]
            public string KeyFile { get; set; }

            [Option('x', "cert-file", Required = false, Default = null, HelpText = "Path to ssl cert file ")]
            public string CertFile { get; set; }
        }
    }
}
