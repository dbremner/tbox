using System;
using Mnk.Library.Common.Communications;
using Mnk.TBox.Plugins.SkyNet.Code.Interfaces;
using Mnk.TBox.Tools.SkyNet.Common;
using Mnk.TBox.Tools.SkyNet.Common.Configurations;

namespace Mnk.TBox.Plugins.SkyNet.Code
{
    public class ServicesBuilder : IServicesBuilder
    {
        public NetworkClient<ISkyNetServerAgentsService> CreateServerAgentsClient(AgentConfig config)
        {
            return new NetworkClient<ISkyNetServerAgentsService>(new Uri(config.ServerEndpoint));
        }

        public NetworkClient<ISkyNetServerTasksService> CreateServerTasksClient(AgentConfig config)
        {
            return new NetworkClient<ISkyNetServerTasksService>(new Uri(config.ServerEndpoint));
        }

        public NetworkClient<ISkyNetFileService> CreateFileServerClient(AgentConfig config)
        {
            return new NetworkClient<ISkyNetFileService>(new Uri(config.ServerEndpoint));
        }

    }
}
