using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PocUnifiedLogWorkerCommon
{
    public class Worker
    {
        public void Work()
        {
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

        public Task ThreadSend(string dir)
        {
            var o = new KinesisWriter();
            return Task.Factory.StartNew(() => o.ProcessDirectory(dir));
        }
    }
}
