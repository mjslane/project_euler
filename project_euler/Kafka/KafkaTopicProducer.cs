using System;
using System.Collections.Generic;
using System.Text;
using Confluent.Kafka;
using Confluent.Kafka.Serialization;

namespace app.Kafka
{
    public class KafkaTopicProducer
    {
        private readonly Producer<Ignore, string> producer;
        private readonly string topic;

        public KafkaTopicProducer(string brokerList, string topic, SslConfig sslConfig)
        {
            var config = new Dictionary<string, object>
            {
                { "bootstrap.servers", brokerList },
                { "group.id", "matt_slane" },
                { "enable.auto.commit", true },  // this is the default
                { "auto.commit.interval.ms", 5000 },
                { "statistics.interval.ms", 60000 },
                { "session.timeout.ms", 6000 },
                { "auto.offset.reset", "earliest" },

            };

            if (sslConfig != null)
            {
                config.Add("security.protocol", "ssl");

                if (!String.IsNullOrWhiteSpace(sslConfig.CaLocation))
                {
                    config.Add("ssl.ca.location", sslConfig.CaLocation);
                }

                if (!String.IsNullOrWhiteSpace(sslConfig.KeyLocation))
                {
                    config.Add("ssl.key.location", sslConfig.KeyLocation);
                }

                if (!String.IsNullOrWhiteSpace(sslConfig.CertificateLocation))
                {
                    config.Add("ssl.certificate.location", sslConfig.CertificateLocation);
                }
            }

            var serialiser = new StringSerializer(Encoding.UTF8);
            this.producer = new Producer<Ignore, string>(config, null, serialiser);

            this.topic = topic;
        }

        public void ProduceMessage(string message)
        {
            var deliveryResult  = this.producer.ProduceAsync(this.topic, null, message).Result;
            if (deliveryResult.Error != null)
            {
                Console.WriteLine($"Failed to deliver '{message}' to topic { deliveryResult.Topic}: deliveryResult.Error");
            }
        }
    }
}
