using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using log4net.Config;
using Newtonsoft.Json;

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

            while (true)
            {
                Thread.Sleep(Properties.Settings.Default.CycleMsec);

                var dirs = Directory.GetDirectories(Properties.Settings.Default.WatchedDir, "*.*", SearchOption.TopDirectoryOnly);

                var tasks = dirs.Select(ThreadSend).ToList();

                Task.WaitAll(tasks.ToArray());

                foreach (var task in tasks)
                {
                    task.Dispose();
                }
            }
        }

        static Task ThreadSend(string dir)
        {
            var o = new KinesisWriter();
            return Task.Factory.StartNew(() => o.ProcessDirectory(dir));
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
                File.WriteAllText(Path.Combine(Properties.Settings.Default.WatchedDir, Guid.NewGuid().ToString().Replace("-", "") + ".json"), ser);
            }
        }
    }
}
