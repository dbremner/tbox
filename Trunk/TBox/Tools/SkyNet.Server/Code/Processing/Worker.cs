using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Mnk.Library.ScriptEngine.Core.Interfaces;
using Mnk.TBox.Tools.SkyNet.Common;
using Mnk.TBox.Tools.SkyNet.Server.Code.Interfaces;

namespace Mnk.TBox.Tools.SkyNet.Server.Code.Processing
{
    class Worker : IWorker
    {
        private readonly IScriptCompiler<ISkyScript> compiler;
        private readonly ISkyContext context;
        private readonly ISkyAgentLogic agentLogic;

        public Worker(IScriptCompiler<ISkyScript> compiler, ISkyContext context, ISkyAgentLogic agentLogic)
        {
            this.compiler = compiler;
            this.context = context;
            this.agentLogic = agentLogic;
        }

        public void ProcessTask(ServerTask task, IList<ServerAgent> agents)
        {
            var script = compiler.Compile(task.Script);
            var items = StartAgents(task, script, agents);

            WaitAgents(items);

            task.Report = BuildResult(script, items);
        }

        private WorkerTask[] StartAgents(ServerTask task, ISkyScript script, IList<ServerAgent> agents)
        {
            var items = script.ServerBuildAgentsData(agents, context)
                .AsParallel()
                .Select(item => agentLogic.CreateWorkerTask(item.Agent, item.Config, task))
                .ToArray();
            return items;
        }

        private void WaitAgents(WorkerTask[] items)
        {
            while (items.AsParallel().All(x=>!agentLogic.IsDone(x)))
            {
                Thread.Sleep(3000);
            }
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
