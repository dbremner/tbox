using System;
using System.Globalization;
using System.ServiceModel;

namespace Mnk.Library.Common.Communications
{
    public sealed class InterprocessServer<T> : IDisposable
    {
        private readonly ServiceHost server;
        public string Handle { get; private set; }
        public T Owner { get; private set; }
        public InterprocessServer(T owner, string handle = null)
        {
            Handle = handle ?? Guid.NewGuid().ToString();
            Owner = owner;
            server = new ServiceHost(owner);
            server.AddServiceEndpoint(typeof(T),
                new NetNamedPipeBinding(NetNamedPipeSecurityMode.None)
                {
                    MaxReceivedMessageSize = int.MaxValue,
                    CloseTimeout = TimeSpan.FromMinutes(10),
                    SendTimeout = TimeSpan.FromMinutes(10),
                    ReceiveTimeout = TimeSpan.MaxValue
                },
                string.Format(CultureInfo.InvariantCulture, "net.pipe://{0}/{1}", Environment.MachineName, Handle));
            server.Open();
        }

        public void Dispose()
        {
            server.Close();
        }
    }
}
