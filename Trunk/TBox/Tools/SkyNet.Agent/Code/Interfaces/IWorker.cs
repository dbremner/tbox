using Mnk.TBox.Tools.SkyNet.Common;

namespace Mnk.TBox.Tools.SkyNet.Agent.Code.Interfaces
{
    public interface IWorker
    {
        void Start(AgentTask task);
        bool IsDone { get; }
        void Cancel();
        void Terminate();
    }
}