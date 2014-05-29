using System.Threading.Tasks;
using Mnk.TBox.Plugins.SkyNet.Code.Interfaces;
using Mnk.TBox.Tools.SkyNet.Common;

namespace Mnk.TBox.Plugins.SkyNet.Code
{
    class SettingsLogic : ISettingsLogic
    {
        private readonly IConfigsFacade configsFacade;
        private readonly IServicesFacade servicesFacade;

        public SettingsLogic(IConfigsFacade configsFacade, IServicesFacade servicesFacade)
        {
            this.configsFacade = configsFacade;
            this.servicesFacade = servicesFacade;
        }

        public AgentConfig AgentConfig
        {
            get
            {
                return configsFacade.AgentConfig;
            }
            set
            {
                configsFacade.AgentConfig = value;
            }
        }

        public ServerConfig ServerConfig
        {
            get
            {
                return configsFacade.ServerConfig;
            }
            set
            {
                configsFacade.ServerConfig = value;
            }
        }

        public SkyNetStatus GetStatus(AgentConfig config)
        {
            var status = new SkyNetStatus();
            Parallel.Invoke(
                () => status.Task = servicesFacade.GetAgentCurrentTask(config),
                () => status.Agents = servicesFacade.GetServiceAgents(config),
                () => status.Tasks = servicesFacade.GetServiceTasks(config)
                );
            return status;
        }
    }
}
