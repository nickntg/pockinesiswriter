using System.ServiceProcess;
using log4net;
using log4net.Config;
using PocUnifiedLogWorkerCommon;

namespace PocUnifiledLogService
{
    public partial class PocService : ServiceBase
    {
        public PocService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            XmlConfigurator.Configure();
            var log = LogManager.GetLogger(typeof (PocService));
            new Worker().WorkAsync();
            log.Info("Started");
        }

        protected override void OnStop()
        {
            var log = LogManager.GetLogger(typeof(PocService));
            log.Info("Stopped");
        }
    }
}
