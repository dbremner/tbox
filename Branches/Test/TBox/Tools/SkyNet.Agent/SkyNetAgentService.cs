using System;
using System.ServiceProcess;
using Common.Communications.Network;
using SkyNet.Common.Agent;

namespace SkyNet.Agent
{
    public partial class SkyNetAgentService : ServiceBase
    {
        public SkyNetAgentService()
        {
            InitializeComponent();
        }

        private IDisposable server;

        protected override void OnStart(string[] args)
        {
            server = new Server<ISkyNetAgent>(new SkyNetAgent(), 11111);
        }

        protected override void OnStop()
        {
            if (server == null) return;
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
