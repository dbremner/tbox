using Mnk.TBox.Tools.SkyNet.Common;

namespace Mnk.TBox.Plugins.SkyNet.Code.Interfaces
{
    public interface ISettingsLogic
    {
        AgentConfig AgentConfig { get; set; }
        ServerConfig ServerConfig { get; set; }
        SkyNetStatus GetStatus(AgentConfig config);
    }
}
