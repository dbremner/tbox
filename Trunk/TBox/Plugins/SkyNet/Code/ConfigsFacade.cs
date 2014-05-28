using System;
using Mnk.Library.Common.Communications;
using Mnk.Library.Common.Log;
using Mnk.TBox.Plugins.SkyNet.Code.Interfaces;
using Mnk.TBox.Tools.SkyNet.Common.Configurations;

namespace Mnk.TBox.Plugins.SkyNet.Code
{
    class ConfigsFacade : IConfigsFacade
    {
        private readonly ILog log = LogManager.GetLogger<ServicesBuilder>();

        private static InterprocessClient<IConfigProvider<AgentConfig>> CreateAgentConfigProvider()
        {
            return new InterprocessClient<IConfigProvider<AgentConfig>>(Constants.AgentServiceName);
        }

        private static InterprocessClient<IConfigProvider<ServerConfig>> CreateServerConfigProvider()
        {
            return new InterprocessClient<IConfigProvider<ServerConfig>>(Constants.ServerServiceName);
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
