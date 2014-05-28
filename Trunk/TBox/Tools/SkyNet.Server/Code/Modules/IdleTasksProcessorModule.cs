using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Mnk.TBox.Tools.SkyNet.Common;
using Mnk.TBox.Tools.SkyNet.Common.Modules;
using Mnk.TBox.Tools.SkyNet.Server.Code.Interfaces;

namespace Mnk.TBox.Tools.SkyNet.Server.Code.Modules
{
    class IdleTasksProcessorModule : IModule
    {
        private readonly IAgentsCache cache;
        private readonly IStorage storage;
        private readonly IWorker worker;
        private readonly ISkyContext context;

        public IdleTasksProcessorModule(IAgentsCache cache, IStorage storage, IWorker worker, ISkyContext context)
        {
            this.cache = cache;
            this.storage = storage;
            this.worker = worker;
            this.context = context;
        }

        public void Process()
        {
            var task = GetNextTask();
            if(task == null)return;
            var agents = GetAgents();
            if(!agents.Any())return;
            PrepareToStart(task, agents);
            try
            {
                worker.ProcessTask(task, agents);
            }
            finally
            {
                PrepareToEnd(task, agents);
            }
        }

        private ServerTask GetNextTask()
        {
            lock (storage)
            {
                return storage.Config.Tasks.FirstOrDefault(x => x.State == TaskState.Idle);
            }
        }

        private IList<ServerAgent> GetAgents()
        {
            lock (storage)
            {
                return storage.Config.Agents
                    .Where(x => x.State == AgentState.Idle)
                    .ToArray();
            }
        }

        private void PrepareToStart(ServerTask task, IEnumerable<ServerAgent> agents)
        {
            cache.Clear();
            context.Reset();
            task.State = TaskState.InProgress;
            foreach (var agent in agents)
            {
                agent.State = AgentState.InProgress;
            }
        }

        private void PrepareToEnd(ServerTask task, IEnumerable<ServerAgent> agents)
        {
            task.State = TaskState.Done;
            foreach (var agent in agents)
            {
                agent.State = AgentState.Idle;
            }
        }

    }
}
