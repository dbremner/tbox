using Mnk.TBox.Tools.SkyNet.Common.Configurations;

namespace Mnk.TBox.Plugins.SkyNet.Code.Interfaces
{
    public interface IConfigsFacade
    {
        AgentConfig GetAgentConfig();
        void SetAgentConfig(AgentConfig config);
        ServerConfig GetServerConfig();
        void SetServerConfig(ServerConfig config);
    }


}
