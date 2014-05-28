using Mnk.Library.Common.Communications;
using Mnk.TBox.Tools.SkyNet.Common;
using Mnk.TBox.Tools.SkyNet.Common.Configurations;

namespace Mnk.TBox.Plugins.SkyNet.Code.Interfaces
{
    public interface IServicesBuilder 
    {
        NetworkClient<ISkyNetServerAgentsService> CreateServerAgentsClient(AgentConfig config);
        NetworkClient<ISkyNetServerTasksService> CreateServerTasksClient(AgentConfig config);
        NetworkClient<ISkyNetFileService> CreateFileServerClient(AgentConfig config);
    }
}