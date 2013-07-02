using System;
using System.ServiceModel;

namespace Common.Communications
{
	[Serializable]
	public sealed class Client<T>
	{
		public T Instance { get; private set; }
		public Client(string handle)
		{
			Instance = ChannelFactory<T>.
				CreateChannel(
					new NetNamedPipeBinding(NetNamedPipeSecurityMode.None) { MaxReceivedMessageSize = int.MaxValue },
					new EndpointAddress(string.Format("net.pipe://{0}/{1}", Environment.MachineName, handle)));
		}

	}
}
