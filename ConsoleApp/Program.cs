using System;
using System.IO;
using log4net.Config;
using Newtonsoft.Json;
using PocUnifiedLogWorkerCommon;

namespace PocUnifiedLogWorker
{
    class Program
    {
        static void Main(string[] args)
        {
            XmlConfigurator.Configure();

            if (args != null && args.Length != 0)
            {
                // If any argument is specified, create test data and quit.
                CreateSamples();
                return;
            }

            var worker = new Worker();
            worker.Work();
        }

        static void CreateSamples()
        {
            var x = new EventData
            {
                KinesisInfo =
                    new KinesisInfo
                    {
                        StreamName = "ul_stream1",
                        Endpoint = "eu-west-1",
                        AccessKey = "yeahright",
                        SecretKey = "yeahright",
                        PartitionKey = "key"
                    },
                UnifiedLogEvent =
                    new UnifiedLogEvent
                    {
                        CreatedAt = DateTime.Now,
                        Data = "{danda}",
                        IpAddress = "10.0.0.1",
                        Name = "da name",
                        Source = "da source",
                        Version = "1.0"
                    }
            };

            var ser = JsonConvert.SerializeObject(x);
            for (var i = 1; i <= 1000; i++)
            {
                File.WriteAllText(Path.Combine(Directory.GetCurrentDirectory(), Guid.NewGuid().ToString().Replace("-", "") + ".json"), ser);
            }
        }
    }
}
