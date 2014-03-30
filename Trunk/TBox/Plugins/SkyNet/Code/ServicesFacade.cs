using System;
using Mnk.Library.Common.Base.Log;
using Mnk.Library.Common.Communications.Interprocess;
using Mnk.Library.Common.Communications.Network;
using Mnk.TBox.Tools.SkyNet.Common.Configurations;
using Mnk.TBox.Tools.SkyNet.Common.Contracts.Files;
using Mnk.TBox.Tools.SkyNet.Common.Contracts.Server;

namespace Mnk.TBox.Plugins.SkyNet.Code
{
    public class ServicesFacade
    {
        private readonly ILog log = LogManager.GetLogger<ServicesFacade>();
        public const string AgentServiceName = "TBox.SkyNet.Agent";
        public const string ServerServiceName = "TBox.SkyNet.Server";

        public NetworkClient<ISkyNetServerService> CreateServerClient(AgentConfig config)
        {
            return new NetworkClient<ISkyNetServerService>(new Uri(config.ServerEndpoint));
        }

        public NetworkClient<ISkyNetFileService> CreateFileServerClient(AgentConfig config)
        {
            return new NetworkClient<ISkyNetFileService>(new Uri(config.ServerEndpoint));
        }

        private static InterprocessClient<IConfigProvider<AgentConfig>> CreateAgentConfigProvider()
        {
            return new InterprocessClient<IConfigProvider<AgentConfig>>(AgentServiceName);
        }

        private static InterprocessClient<IConfigProvider<ServerConfig>> CreateServerConfigProvider()
        {
            return new InterprocessClient<IConfigProvider<ServerConfig>>(ServerServiceName);
        }

        private void SetConfig<T>(InterprocessClient<IConfigProvider<T>> cl, object config)
            where T : class
        {
            if (config == null) return;
            try
            {
                cl.Instance.Set((T)config);
            }
            catch (Exception ex)
            {
                log.Write(ex, "Can't update configuration");
            }
        }

        private T GetConfig<T>(InterprocessClient<IConfigProvider<T>> cl)
            where T : class
        {
            try
            {
                return cl.Instance.Get();
            }
            catch (Exception ex)
            {
                log.Write(ex, "Can't get configuration");
            }
            return null;
        }

        public AgentConfig GetAgentConfig()
        {
            using (var cl = CreateAgentConfigProvider())
            {
                return GetConfig(cl);
            }
        }

        public void SetAgentConfig(AgentConfig config)
        {
            using (var cl = CreateAgentConfigProvider())
            {
                SetConfig(cl, config);
            }
        }

        public ServerConfig GetServerConfig()
        {
            using (var cl = CreateServerConfigProvider())
            {
                return GetConfig(cl);
            }
        }

        public void SetServerConfig(ServerConfig config)
        {
            using (var cl = CreateServerConfigProvider())
            {
                SetConfig(cl, config);
            }
        }

    }
}
