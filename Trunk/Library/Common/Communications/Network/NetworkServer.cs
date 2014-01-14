using System;
using System.ServiceModel;
using System.ServiceModel.Description;

namespace Mnk.Library.Common.Communications.Network
{
    public sealed class NetworkServer<T> : IDisposable
    {
        private readonly ServiceHost server;
        public int Port { get; private set; }
        public string Endpoint { get; private set; }
        public T Owner { get; private set; }
        public NetworkServer(T owner, int port)
        {
            Port = port;
            Owner = owner;
            Endpoint = string.Format("http://{0}:{1}", Environment.MachineName, port);
            server = new ServiceHost(owner);
            var ep = server.AddServiceEndpoint(typeof(T),
                new WebHttpBinding { Security = new WebHttpSecurity { Mode = WebHttpSecurityMode.None } },
                Endpoint);
            ep.Behaviors.Add(new WebHttpBehavior());
            server.Open();
        }

        public void Dispose()
        {
            server.Close();
        }
    }
}
