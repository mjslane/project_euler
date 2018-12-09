using System;
using System.Collections.Generic;
using System.Text;
using Confluent.Kafka;
using Confluent.Kafka.Serialization;

namespace app.Kafka
{
    public class KafkaTopicProducer : KafkaClient
    {
        private readonly Producer<string, string> producer;
        private readonly string topic;

        public KafkaTopicProducer(string brokerList, string topic, SslConfig sslConfig) 
        {
            var config = new Dictionary<string, object>
            {
                { "bootstrap.servers", brokerList },
                { "group.id", "euler" },
                { "enable.auto.commit", true },  // this is the default
                { "auto.commit.interval.ms", 5000 },
                { "statistics.interval.ms", 60000 },
                { "session.timeout.ms", 6000 }
            };

            SetSslConfig(sslConfig, config);
            var keySerialiser = new StringSerializer(Encoding.UTF8);
            var valueSerialiser = new StringSerializer(Encoding.UTF8);
            this.producer = new Producer<string, string>(config, keySerialiser, valueSerialiser);

            this.topic = topic;
        }

        public void ProduceMessage(string key, string message)
        {
            var deliveryResult  = this.producer.ProduceAsync(this.topic, key, message).Result;
            if (deliveryResult.Error != null)
            {
                Console.WriteLine($"Failed to deliver '{message}' to topic { deliveryResult.Topic}: deliveryResult.Error");
            }
        }
    }
}
