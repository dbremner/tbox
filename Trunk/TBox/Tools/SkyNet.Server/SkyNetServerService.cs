using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using Mnk.TBox.Tools.SkyNet.Common.Configurations;
using Mnk.TBox.Tools.SkyNet.Common.Contracts.Server;
using Mnk.Library.Common.Communications.Network;

namespace Mnk.TBox.Tools.SkyNet.Server
{
    public partial class SkyNetServerService : ServiceBase
    {
        private readonly ServerConfig config;

        public SkyNetServerService(ServerConfig config)
        {
            this.config = config;
            InitializeComponent();
        }

        private IDisposable server;

        protected override void OnStart(string[] args)
        {
            server = new NetworkServer<ISkyNetServer>(new SkyNetServer(config), config.Port);
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
