using System;
using System.ServiceModel;
using System.ServiceModel.Description;

namespace Common.Communications.Network
{
	public sealed class Server<T> : IDisposable
	{
		private readonly ServiceHost server;
		public int Port { get; private set; }
		public T Owner { get; private set; }
		public Server(T owner, int port)
		{
			Port = port;
			Owner = owner;
			server = new ServiceHost(owner);
			var ep = server.AddServiceEndpoint(typeof(T),
				new WebHttpBinding { Security = new WebHttpSecurity { Mode = WebHttpSecurityMode.None } },
				string.Format("http://{0}:{1}", Environment.MachineName, port));
			ep.Behaviors.Add(new WebHttpBehavior());
			server.Open();
		}

		public void Dispose()
		{
			server.Close();
		}
	}
}
