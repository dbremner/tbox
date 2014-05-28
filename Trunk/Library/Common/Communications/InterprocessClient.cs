﻿using System;
using System.Globalization;
using System.ServiceModel;

namespace Mnk.Library.Common.Communications
{
    [Serializable]
    public sealed class InterprocessClient<T> : IDisposable
    {
        public T Instance { get; private set; }
        public InterprocessClient(string handle)
        {
            Instance = ChannelFactory<T>.
                CreateChannel(
                    new NetNamedPipeBinding(NetNamedPipeSecurityMode.None) { MaxReceivedMessageSize = int.MaxValue },
                    new EndpointAddress(string.Format(CultureInfo.InvariantCulture, "net.pipe://{0}/{1}", Environment.MachineName, handle)));
        }

        public void Dispose()
        {
            ((IClientChannel)Instance).Dispose();
        }
    }
}