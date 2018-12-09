using System.Linq;
using System.IO;
using CommandLine;
using System;
using app.Kafka;
using System.Collections.Generic;
using System.Threading;
using Confluent.Kafka;
using app.Euler;

namespace project_euler
{
    class Program
    {
        private static List<string> kafkaJobsTopics = new List<string>();
        private static string kafkaAnswerTopic;
        private static string kafkaBroker;
        private static SslConfig sslConfig;
        private static KafkaTopicPublisher kafkaTopicPublisher;
      

        static void Main(string[] args)
        {
            GetConfiguration(args);

            CancellationTokenSource cts = new CancellationTokenSource();
            Console.CancelKeyPress += (_, e) => { e.Cancel = true; cts.Cancel(); };
            var consumer = new KafkaTopicConsumer(kafkaBroker, kafkaJobsTopics, sslConfig);
            kafkaTopicPublisher = new KafkaTopicPublisher(kafkaBroker, kafkaAnswerTopic, sslConfig);
            consumer.Consume(writeMessage, cts.Token);
        }

        private static void GetConfiguration(string[] args)
        {
            var parsedArgs = Parser.Default.ParseArguments<Options>(args);
            ParseFileOptions(parsedArgs);
            GetEnvironmentOptions();
            GetCommandLineOptions(parsedArgs);
        }

        private static void setTopicsFromStringList(string value)
        {
            kafkaJobsTopics = value.Split(',').ToList();
        }

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

        private static void ParseFileOptions(ParserResult<Options> options)
        {
            options.WithParsed<Options>(opts =>
            {
                if (!string.IsNullOrWhiteSpace(opts.ConfigurationFileName))
                {
                    Console.WriteLine($"Reading options from {opts.ConfigurationFileName}");
                    parseConfigFile(opts.ConfigurationFileName);
                }
            });
        }

        private static void parseConfigFile(string configurationFileName)
        {
            if (File.Exists(configurationFileName))
            {
                var lines = File.ReadAllLines(configurationFileName);
                Array.ForEach(lines, item =>
               {
                   string[] kvp = item.Split('=');
                   switch (kvp[0])
                   {
                       case "KAFKA_BROKER":
                           kafkaBroker = kvp[1];
                           break;
                       case "KAFKA_JOBS_TOPIC":
                           setTopicsFromStringList(kvp[1]);
                           break;
                       case "KAFKA_ANSWER_TOPIC":
                           kafkaAnswerTopic = kvp[1];
                           break;
                       case "CERT_FILE_LOCATION":
                       case "CA_FILE_LOCATION":
                       case "KEY_FILE_LOCATION":
                           writeSSLValue(kvp[0], kvp[1]);
                           break;
                       default:
                           break;
                   }
               });
            }
            else
            {
                throw new FileNotFoundException($"{configurationFileName} does not exist");
            }
        }

        private static void GetEnvironmentOptions()
        {
            var val = Environment.GetEnvironmentVariable("KAFKA_BROKER");
            if (!String.IsNullOrWhiteSpace(val))
            {
                kafkaBroker = val;
            }

            val = Environment.GetEnvironmentVariable("KAFKA_JOBS_TOPIC");
            if (!String.IsNullOrWhiteSpace(val))
            {
                setTopicsFromStringList(val);
            }

            val = Environment.GetEnvironmentVariable("KAFKA_ANSWER_TOPIC");
            if (!String.IsNullOrWhiteSpace(val))
            {
                kafkaAnswerTopic = val;
            }

            val = Environment.GetEnvironmentVariable("CERT_FILE_LOCATION");
            if (!String.IsNullOrWhiteSpace(val))
            {
                writeSSLValue("CERT_FILE_LOCATION", val);
            }

            val = Environment.GetEnvironmentVariable("CA_FILE_LOCATION");
            if (!String.IsNullOrWhiteSpace(val))
            {
                writeSSLValue("CA_FILE_LOCATION", val);
            }

            val = Environment.GetEnvironmentVariable("KEY_FILE_LOCATION");
            if (!String.IsNullOrWhiteSpace(val))
            {
                writeSSLValue("KEY_FILE_LOCATION", val);
            }
        }

        private static void GetCommandLineOptions(ParserResult<Options> options)
        {
            options.WithParsed<Options>(opts =>
            {
                if (!String.IsNullOrWhiteSpace(opts.Brokers))
                {
                    kafkaBroker = opts.Brokers;
                }

                if (!String.IsNullOrWhiteSpace(opts.JobTopic))
                {
                    setTopicsFromStringList(opts.JobTopic);
                }

                if (!String.IsNullOrWhiteSpace(opts.AnswerTopic))
                {
                    kafkaAnswerTopic = opts.AnswerTopic;
                }

                if (!String.IsNullOrWhiteSpace(opts.CaFile))
                {
                    writeSSLValue("CA_FILE_LOCATION", opts.CaFile);
                }

                if (!String.IsNullOrWhiteSpace(opts.CertFile))
                {
                    writeSSLValue("CERT_FILE_LOCATION", opts.CertFile);
                }

                if (!String.IsNullOrWhiteSpace(opts.KeyFile))
                {
                    writeSSLValue("KEY_FILE_LOCATION", opts.KeyFile);
                }
            });
        }

        private static void writeSSLValue(string key, string value)
        {
            if(sslConfig == null)
            {
                sslConfig = new SslConfig();
            }

            if (key == "CERT_FILE_LOCATION") sslConfig.CertificateLocation = value;
            if (key == "CA_FILE_LOCATION") sslConfig.CaLocation = value;
            if (key == "KEY_FILE_LOCATION") sslConfig.KeyLocation = value;
        }

        private static void writeMessage(Message<Ignore, string> obj)
        {
            int value;
            if (int.TryParse(obj.Value, out value))
            {
                int sum = Euler.Sum(value);
                 
                kafkaTopicPublisher.PublishMessage($"{value}: {sum}");
            }
            else
            {
                Console.WriteLine($"Error parsing {obj.Value} to Int");
            }
        }
    }
}
