using System;
using System.Globalization;
using System.ServiceModel;
using System.ServiceModel.Description;

namespace Mnk.Library.Common.Communications
{

    [Serializable]
    public sealed class NetworkClient<T> : IDisposable
    {
        public T Instance { get; private set; }

        public NetworkClient(string machineName, int port) :
            this(new Uri(string.Format(CultureInfo.InvariantCulture, "http://{0}:{1}", machineName, port))) { }

        public NetworkClient(Uri uri)
        {
            var binding = new WebHttpBinding
            {
                Security = new WebHttpSecurity {Mode = WebHttpSecurityMode.None},
                TransferMode = TransferMode.Streamed,
                MaxReceivedMessageSize = int.MaxValue,
                ReaderQuotas =
                {
                    MaxArrayLength = int.MaxValue, 
                    MaxStringContentLength = int.MaxValue,
                }
            };
            var factory = new ChannelFactory<T>( binding, new EndpointAddress(uri));
            factory.Endpoint.Behaviors.Add(new WebHttpBehavior());
            Instance = factory.CreateChannel();
        }

        public void Dispose()
        {
            ((IClientChannel)Instance).Dispose();
        }
    }
}
