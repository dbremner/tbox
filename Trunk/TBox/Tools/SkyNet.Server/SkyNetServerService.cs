using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using SkyNet.Common.Server;
using Common.Communications.Network;

namespace SkyNet.Server
{
    public partial class SkyNetServerService : ServiceBase
    {
        public SkyNetServerService()
        {
            InitializeComponent();
        }

        private IDisposable server;

        protected override void OnStart(string[] args)
        {
            server = new Server<ISkyNetServer>(new SkyNetServer(), 11111);
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
