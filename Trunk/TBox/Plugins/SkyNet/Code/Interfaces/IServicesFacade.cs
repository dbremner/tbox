using System.Collections.Generic;
using Mnk.TBox.Tools.SkyNet.Common;

namespace Mnk.TBox.Plugins.SkyNet.Code.Interfaces
{
    public interface IServicesFacade
    {
        IList<ServerTask> GetServiceTasks();
        IList<ServerAgent> GetServiceAgents();
        AgentTask GetAgentCurrentTask();
        SkyNetStatus GetStatus();
        string UploadFile(string path);
        string StartTask(ServerTask task);
        void Cancel(string id);
        void Terminate(string id);
        void DeleteTask(string id);
    }
}
