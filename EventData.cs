namespace PocUnifiedLogWorker
{
    public class EventData
    {
        public UnifiedLogEvent UnifiedLogEvent { get; set; }
        public KinesisInfo KinesisInfo { get; set; }
    }
}
