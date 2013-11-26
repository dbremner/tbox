using System;
using System.ServiceProcess;
using Common.Base.Log;
using Common.Communications.Network;
using SkyNet.Common.Configurations;
using SkyNet.Common.Contracts.Agent;
using SkyNet.Common.Contracts.Server;

namespace SkyNet.Agent
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

        private NetworkServer<ISkyNetAgent> server;

        protected override void OnStart(string[] args)
        {
            server = new NetworkServer<ISkyNetAgent>(new SkyNetAgent(config), config.Port);
            try
            {
                var cl = new NetworkClient<ISkyNetServer>(new Uri(config.ServerEndpoint));
                cl.Instance.ConnectAgent(new ServerAgent
                {
                    TotalCores = config.TotalCores,
                    Endpoint = server.Endpoint
                });
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
                var cl = new NetworkClient<ISkyNetServer>(new Uri(config.ServerEndpoint));
                cl.Instance.DisconnectAgent(server.Endpoint);
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
