using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mnk.TBox.Tools.SkyNet.Common;
using Mnk.TBox.Tools.SkyNet.Common.Modules;
using Mnk.TBox.Tools.SkyNet.Server.Code.Interfaces;

namespace Mnk.TBox.Tools.SkyNet.Server.Code.Modules
{
    class HandleDiedAgents : IModule
    {
        private readonly IStorage storage;
        private readonly ISkyAgentLogic agentLogic;

        public HandleDiedAgents(IStorage storage, ISkyAgentLogic agentLogic)
        {
            this.storage = storage;
            this.agentLogic = agentLogic;
        }

        public void Process()
        {
            var agents = GetAgents();
            if (!agents.Any())return;
            RemoveDied(agents);
            if (!agents.Any()) return;
            ResetNonIdle(agents);
        }

        private void ResetNonIdle(IEnumerable<ServerAgent> agents)
        {
            var nonIdleAgents = agents.Where(x => x.State != AgentState.Idle).ToArray();
            Parallel.ForEach(nonIdleAgents, agent =>
            {
                var id = agentLogic.TerminateCurrentTask(agent);
                agentLogic.DeleteTask(agent, id);
                agent.State = AgentState.Idle;
            });
        }

        private void RemoveDied(IList<ServerAgent> agents)
        {
            var toRemove = agents
                .AsParallel()
                .Where(a => !agentLogic.IsAlive(a))
                .ToArray();
            if (!toRemove.Any()) return;
            RemoveAgents(toRemove);
            foreach (var agent in toRemove)
            {
                agents.Remove(agent);
            }
        }

        private void RemoveAgents(IEnumerable<ServerAgent> toRemove)
        {
            lock (storage)
            {
                foreach (var agent in toRemove)
                {
                    storage.Config.Agents.Remove(agent);
                }
            }
        }

        private IList<ServerAgent> GetAgents()
        {
            lock (storage)
            {
                return storage.Config.Agents.ToArray();
            }
        }

    }
}
