using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Confluent.Kafka;
using Confluent.Kafka.Serialization;

namespace app.Kafka
{
    public class KafkaTopicConsumer
    {
        private readonly Consumer<Ignore, string> consumer;
        private readonly List<string> topics;

        public KafkaTopicConsumer(string brokerList, List<string> topics)
        {

            var config = new Dictionary<string, object>
            {
                { "bootstrap.servers", brokerList },
                { "group.id", "csharp-consumer" },
                { "enable.auto.commit", true },  // this is the default
                { "auto.commit.interval.ms", 5000 },
                { "statistics.interval.ms", 60000 },
                { "session.timeout.ms", 6000 },
                { "auto.offset.reset", "smallest" }
            };

            this.consumer = new Consumer<Ignore, string>(config, null, new StringDeserializer());
            this.topics = topics;
        }

        public void Consume(Action<Message<Ignore, string>> messageOperator, CancellationToken cancellationToken)
        {
            this.consumer.OnMessage += (_, msg) => messageOperator(msg);
            consumer.OnPartitionEOF += (_, end) => Console.WriteLine($"Reached end of topic {end.Topic} partition {end.Partition}, next message will be at offset {end.Offset}");

            consumer.OnError += (_, error) => Console.WriteLine($"Error: {error}");

            consumer.OnConsumeError += (_, msg) => Console.WriteLine($"Error consuming from topic/partition/offset {msg.Topic}/{msg.Partition}/{msg.Offset}: {msg.Error}");

            consumer.OnOffsetsCommitted += (_, commit) => Console.WriteLine(
                        commit.Error ? $"Failed to commit offsets: {commit.Error}" : $"Successfully committed offsets: [{string.Join(", ", commit.Offsets)}]"
                    );

            consumer.Subscribe(this.topics);

            while (!cancellationToken.IsCancellationRequested)
            {
                consumer.Poll(TimeSpan.FromMilliseconds(500));
            }
        }
    }
}