using System.Collections.Generic;
using Mnk.TBox.Tools.SkyNet.Common;

namespace Mnk.TBox.Plugins.SkyNet.Code.Interfaces
{
    interface IServicesFacade
    {
        IList<ServerTask> GetServiceTasks(AgentConfig config);
        IList<ServerAgent> GetServiceAgents(AgentConfig config);
        AgentTask GetAgentCurrentTask(AgentConfig config);
        string UploadFile(AgentConfig config, string path);
        string StartTask(AgentConfig config, ServerTask task);
    }
}
