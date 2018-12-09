using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Confluent.Kafka;
using Confluent.Kafka.Serialization;

namespace app.Kafka
{
    public class KafkaTopicConsumer : KafkaClient
    {
        private readonly Consumer<string, string> consumer;
        private readonly List<string> topics;

        public KafkaTopicConsumer(string brokerList, List<string> topics, SslConfig sslConfig)
        {
            var config = new Dictionary<string, object>
            {
                { "bootstrap.servers", brokerList },
                { "group.id", "euler" },
                { "enable.auto.commit", false },
                { "auto.commit.interval.ms", 5000 },
                { "statistics.interval.ms", 60000 },
                { "session.timeout.ms", 6000 },
                { "auto.offset.reset", "earliest" }
            };
            SetSslConfig(sslConfig, config);

            var keyDeserialiser = new StringDeserializer(Encoding.UTF8);
            var valueDeserialiser = new StringDeserializer(Encoding.UTF8);
            this.consumer = new Consumer<string, string>(config, keyDeserialiser, valueDeserialiser);

            this.topics = topics;
        }

        public void Consume(Action<Message<string, string>> messageOperator, CancellationToken cancellationToken)
        {
            this.consumer.OnMessage += (_, msg) => messageOperator(msg);
            consumer.OnPartitionEOF += (_, end) => Console.WriteLine($"Reached end of topic {end.Topic} partition {end.Partition}, next message will be at offset {end.Offset}");

            consumer.OnError += (_, error) => Console.WriteLine($"Error: {error}");

            consumer.OnConsumeError += (_, msg) => Console.WriteLine($"Error consuming from topic/partition/offset {msg.Topic}/{msg.Partition}/{msg.Offset}: {msg.Error}");

            consumer.OnOffsetsCommitted += (_, commit) => Console.WriteLine(
                        commit.Error ? $"Failed to commit offsets: {commit.Error}" : $"Successfully committed offsets: [{string.Join(", ", commit.Offsets)}]"
                    );

            consumer.OnPartitionsAssigned += (_, p) => {
                List<TopicPartitionOffset> offsets = new List<TopicPartitionOffset>();
                p.ForEach(p1 => offsets.Add(new TopicPartitionOffset(p1, Offset.Beginning)));
                consumer.Assign(offsets);
            };

            consumer.Subscribe(this.topics);

            while (!cancellationToken.IsCancellationRequested)
            {
                consumer.Poll(TimeSpan.FromMilliseconds(500));
            }
        }
    }
}