using System;
using System.Collections.Generic;

namespace app.Kafka
{
    public class KafkaClient
    {
    
        protected static void SetSslConfig(SslConfig sslConfig, Dictionary<string, object> config)
        {
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
        }
    }
}