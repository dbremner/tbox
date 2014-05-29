using System;
using Mnk.Library.Common.Communications;
using Mnk.Library.Common.Log;
using Mnk.TBox.Plugins.SkyNet.Code.Interfaces;
using Mnk.TBox.Tools.SkyNet.Common;

namespace Mnk.TBox.Plugins.SkyNet.Code
{
    class ConfigsFacade : IConfigsFacade
    {
        private readonly ILog log = LogManager.GetLogger<ConfigsFacade>();

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
                cl.Instance.UpdateConfig((T)config);
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
                return cl.Instance.ReceiveConfig();
            }
            catch (Exception ex)
            {
                log.Write(ex, "Can't get configuration");
            }
            return null;
        }

        public AgentConfig AgentConfig
        {
            get
            {
                using (var cl = CreateAgentConfigProvider())
                {
                    return GetConfig(cl);
                }
            }
            set
            {
                using (var cl = CreateAgentConfigProvider())
                {
                    SetConfig(cl, value);
                }
            }
        }

        public ServerConfig ServerConfig
        {
            get
            {
                using (var cl = CreateServerConfigProvider())
                {
                    return GetConfig(cl);
                }
            }
            set
            {
                using (var cl = CreateServerConfigProvider())
                {
                    SetConfig(cl, value);
                }
            }
        }
    }
}
