namespace PocUnifiedLogWorker
{
    public class KinesisInfo
    {
        public string StreamName { get; set; }
        public string PartitionKey { get; set; }
        public string AccessKey { get; set; }
        public string SecretKey { get; set; }
        public string Endpoint { get; set; }
    }
}
