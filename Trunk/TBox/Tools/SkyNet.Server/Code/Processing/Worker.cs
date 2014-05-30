using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Mnk.Library.ScriptEngine.Core.Interfaces;
using Mnk.TBox.Tools.SkyNet.Common;
using Mnk.TBox.Tools.SkyNet.Server.Code.Interfaces;
using ScriptEngine.Core.Params;
using ServiceStack.Text;

namespace Mnk.TBox.Tools.SkyNet.Server.Code.Processing
{
    class Worker : IWorker
    {
        private readonly IScriptCompiler<ISkyScript> compiler;
        private readonly ISkyAgentLogic agentLogic;

        public Worker(IScriptCompiler<ISkyScript> compiler, ISkyAgentLogic agentLogic)
        {
            this.compiler = compiler;
            this.agentLogic = agentLogic;
        }

        public void ProcessTask(ServerTask task, IList<ServerAgent> agents)
        {
            var script = compiler.Compile(task.Script, JsonSerializer.DeserializeFromString<IList<Parameter>>(task.ScriptParameters));
            var items = StartAgents(task, script, agents);
            if (!items.Any())
            {
                throw new ArgumentException("Please divide tasks, 0 agents is not applicable.");
            }

            WaitAgents(task, items);

            task.Report = BuildResult(script, items);
        }

        private WorkerTask[] StartAgents(ServerTask task, ISkyScript script, IList<ServerAgent> agents)
        {
            var items = script.ServerBuildAgentsData(agents)
                .AsParallel()
                .Select(item => agentLogic.CreateWorkerTask(item.Agent, item.Config, task))
                .ToArray();
            return items;
        }

        private void WaitAgents(ServerTask task, IList<WorkerTask> items)
        {
            while(true)
            {
                var tasks = items.AsParallel().Select(x => agentLogic.GetTask(x)).ToArray();
                task.Progress = tasks.Sum(x => GetProgress(x))/tasks.Length;
                if(tasks.All(x => x.IsDone || x.IsCanceled))return;
                Thread.Sleep(3000);
            }
        }

        private static int GetProgress(AgentTask task)
        {
            if (task == null || task.IsCanceled || task.IsDone) return 100;
            return task.Progress;
        }

        private string BuildResult(ISkyScript script, IEnumerable<WorkerTask> items)
        {
            return script.ServerBuildResultByAgentResults(items
                .AsParallel()
                .Select(agentLogic.BuildReport)
                .ToArray()
                );
        }
    }
}
