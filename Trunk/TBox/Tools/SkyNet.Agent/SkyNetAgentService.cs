using System;
using System.ServiceProcess;
using Mnk.Library.Common.Communications;
using Mnk.Library.Common.Log;
using Mnk.TBox.Tools.SkyNet.Common.Configurations;
using Mnk.TBox.Tools.SkyNet.Common.Contracts.Agent;
using Mnk.TBox.Tools.SkyNet.Common.Contracts.Server;

namespace Mnk.TBox.Tools.SkyNet.Agent
{
    public partial class SkyNetAgentService : ServiceBase
    {
        private readonly ILog log = LogManager.GetLogger<SkyNetAgentService>();
        private readonly AgentConfig config;

        public SkyNetAgentService(AgentConfig config)
        {
            this.config = config;
            InitializeComponent();
        }

        private NetworkServer<ISkyNetAgentService> server;

        protected override void OnStart(string[] args)
        {
            server = new NetworkServer<ISkyNetAgentService>(new SkyNetAgent(config), config.Port);
            try
            {
                using (var cl = new NetworkClient<ISkyNetServerService>(new Uri(config.ServerEndpoint)))
                {
                    cl.Instance.ConnectAgent(new ServerAgent
                    {
                        TotalCores = config.TotalCores,
                        Endpoint = server.Endpoint
                    });
                }
            }
            catch (Exception ex)
            {
                log.Write(ex, "Can't connect to server");
            }
        }

        protected override void OnStop()
        {
            if (server == null) return;
            try
            {
                using (var cl = new NetworkClient<ISkyNetServerService>(new Uri(config.ServerEndpoint)))
                {
                    cl.Instance.DisconnectAgent(server.Endpoint);
                }
            }
            catch (Exception ex)
            {
                log.Write(ex, "Can't disconnect fro server");
            }
            server.Dispose();
            server = null;
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
