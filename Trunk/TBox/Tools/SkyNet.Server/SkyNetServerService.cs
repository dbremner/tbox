using System;
using System.ServiceProcess;
using Mnk.Library.Common.Communications;
using Mnk.Library.Common.Log;
using Mnk.TBox.Tools.SkyNet.Common.Configurations;
using Mnk.TBox.Tools.SkyNet.Server.Code.Services;

namespace Mnk.TBox.Tools.SkyNet.Server
{
    public partial class SkyNetServerService : ServiceBase
    {
        private readonly ILog log = LogManager.GetLogger<SkyNetServerService>();
        private readonly ServerConfig config;

        public SkyNetServerService(ServerConfig config)
        {
            this.config = config;
            InitializeComponent();
        }

        private IDisposable server;

        protected override void OnStart(string[] args)
        {
            try
            {
                server = new NetworkServer<ISkyNetCommon>(new SkyNetServer(config), config.Port);
            }
            catch (Exception ex)
            {
                log.Write(ex, "Can't start");
                throw;
            }
        }

        protected override void OnStop()
        {
            try
            {
                StopService(ref server);
            }
            catch (Exception ex)
            {
                log.Write(ex, "Can't stop");
                throw;
            }
        }

        private void StopService(ref IDisposable o)
        {
            if (o == null) return;
            o.Dispose();
            o = null;
        }

        public void StartService()
        {
            OnStart(new string[0]);
        }

        public void StopService()
        {
            OnStop();
        }
    }
}
