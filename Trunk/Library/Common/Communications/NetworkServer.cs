using System;
using System.Globalization;
using System.ServiceModel;
using System.ServiceModel.Description;

namespace Mnk.Library.Common.Communications
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
            Endpoint = BuildEndpoint(port);
            server = new ServiceHost(owner);
            var binding = new WebHttpBinding
            {
                Security = new WebHttpSecurity { Mode = WebHttpSecurityMode.None },
                TransferMode = TransferMode.Streamed,
                MaxReceivedMessageSize = int.MaxValue,
                ReaderQuotas =
                {
                    MaxArrayLength = int.MaxValue,
                    MaxStringContentLength = int.MaxValue,
                }
            };
            var ep = server.AddServiceEndpoint(typeof(T), binding, Endpoint);
            ep.Behaviors.Add(new WebHttpBehavior());
            server.Open();
        }

        public static string BuildEndpoint(int port)
        {
            return string.Format(CultureInfo.InvariantCulture, "http://{0}:{1}", Environment.MachineName, port);
        }

        public void Dispose()
        {
            server.Close();
        }
    }
}
