﻿using System;
using System.ServiceModel;
using System.ServiceModel.Description;

namespace Common.Communications.Network
{
	[Serializable]
	public sealed class NetworkClient<T>
	{
		public T Instance { get; private set; }

	    public NetworkClient(string machineName, int port) : 
            this(new Uri(string.Format("http://{0}:{1}", machineName, port))){}

	    public NetworkClient(Uri uri)
		{
			var factory = new ChannelFactory<T>(
				new WebHttpBinding { Security = new WebHttpSecurity { Mode = WebHttpSecurityMode.None } }, 
				new EndpointAddress(uri));
			factory.Endpoint.Behaviors.Add(new WebHttpBehavior());
			Instance = factory.CreateChannel();
		}

	}
}