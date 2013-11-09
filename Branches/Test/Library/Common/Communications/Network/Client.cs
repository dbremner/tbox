using System;
using System.ServiceModel;
using System.ServiceModel.Description;

namespace Common.Communications.Network
{
	[Serializable]
	public sealed class Client<T>
	{
		public T Instance { get; private set; }
		public Client(Uri uri)
		{
			var factory = new ChannelFactory<T>(
				new WebHttpBinding { Security = new WebHttpSecurity { Mode = WebHttpSecurityMode.None } }, 
				new EndpointAddress(uri));
			factory.Endpoint.Behaviors.Add(new WebHttpBehavior());
			Instance = factory.CreateChannel();
		}

	}
}
