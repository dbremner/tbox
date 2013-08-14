using System;
using System.ServiceModel;

namespace Common.Communications.Interprocess
{
	public sealed class Server<T> : IDisposable
	{
		private readonly ServiceHost server;
		public string Handle { get; private set; }
		public T Owner { get; private set; }
		public Server(T owner)
		{
			Handle = Guid.NewGuid().ToString();
			Owner = owner;
			server = new ServiceHost(owner);
			server.AddServiceEndpoint(typeof(T),
				new NetNamedPipeBinding(NetNamedPipeSecurityMode.None){MaxReceivedMessageSize = int.MaxValue}, 
				string.Format("net.pipe://{0}/{1}", Environment.MachineName, Handle));
			server.Open();
		}

		public void Dispose()
		{
			server.Close();
		}
	}
}
