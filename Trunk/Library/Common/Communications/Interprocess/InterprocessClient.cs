﻿using System;
using System.ServiceModel;

namespace Common.Communications.Interprocess
{
	[Serializable]
	public sealed class InterprocessClient<T>
	{
		public T Instance { get; private set; }
		public InterprocessClient(string handle)
		{
			Instance = ChannelFactory<T>.
				CreateChannel(
					new NetNamedPipeBinding(NetNamedPipeSecurityMode.None) { MaxReceivedMessageSize = int.MaxValue },
					new EndpointAddress(string.Format("net.pipe://{0}/{1}", Environment.MachineName, handle)));
		}

	}
}