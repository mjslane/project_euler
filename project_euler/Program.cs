using System.Linq;
using System.IO;
using CommandLine;
using System;
using app.Kafka;
using System.Collections.Generic;
using System.Threading;
using Confluent.Kafka;

namespace project_euler
{
    class Program
    {
        private static List<string> kafkaJobsTopics = new List<string>();
        private static string kafkaAnswerTopic;
        private static string kafkaBroker;


        public class Options
        {
            [Option('f', "file", Required = false, Default = null, HelpText = "Path to file containing configuration options")]
            public string ConfigurationFileName { get; set; }
        }

        static void Main(string[] args)
        {
            ParseOptions(args);

            CancellationTokenSource cts = new CancellationTokenSource();
            Console.CancelKeyPress += (_, e) => { e.Cancel = true; cts.Cancel(); };
            var consumer = new KafkaTopicConsumer(kafkaBroker, kafkaJobsTopics);
            consumer.Consume(writeMessage, cts.Token);

        }

        private static void writeMessage(Message<Ignore, string> obj)
        {
            Console.WriteLine($"{obj.ToString()} {obj.Key} {obj.Value} {obj.Timestamp}");
        }

        private static void ParseOptions(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args).WithParsed<Options>(options =>
            {
                if (!string.IsNullOrEmpty(options.ConfigurationFileName))
                {
                    Console.WriteLine($"Reading options from {options.ConfigurationFileName}");
                    parseConfigFile(options.ConfigurationFileName);
                }
                else
                {
                    kafkaJobsTopics.Add ("interview_mattslane_jobs");
                    kafkaAnswerTopic = "interview_mattslane_answers";
                    kafkaBroker = "kafka-1703877a-duncan-bc6e.aivencloud.com:24308";
                }
            });

            Console.WriteLine($"Jobs topic: {kafkaJobsTopics}");
            Console.WriteLine($"Answer topic: {kafkaAnswerTopic}");
            Console.WriteLine($"Broker: {kafkaBroker}");
        }

        private static void parseConfigFile(string configurationFileName)
        {
            if (File.Exists(configurationFileName))
            { 
                Array.ForEach(File.ReadAllLines(configurationFileName), item =>
               {
                   string[] kvp = item.Split('=');
                   switch (kvp[0])
                   {
                       case "KAFKA_BROKER":
                           kafkaBroker = kvp[1];
                           break;
                       case "KAFKA_JOBS_TOPIC":
                           kafkaJobsTopics = kvp[1].Split(',').ToList();
                           break;
                       case "KAFKA_ANSWER_TOPIC":
                           kafkaAnswerTopic = kvp[1];
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
    }
}
