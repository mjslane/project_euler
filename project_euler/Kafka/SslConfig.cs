namespace app.Kafka
{
    public class SslConfig
    {
        internal string KeyLocation { get; set; }
        internal string CertificateLocation { get; set; }
        internal string CaLocation { get; set; }
    }
}