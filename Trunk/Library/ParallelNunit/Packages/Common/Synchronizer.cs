using System.Collections.Generic;
using System.Threading;
using Mnk.Library.Common.Communications;
using Mnk.Library.Common.Models;
using Mnk.Library.ParallelNUnit.Contracts;

namespace Mnk.Library.ParallelNUnit.Packages.Common
{
    public class Synchronizer : ISynchronizer
    {
        private readonly IList<Pair<string, bool>> finishedAgents = new List<Pair<string, bool>>();

        public int Finished { get { return finishedAgents.Count; } }

        public void ProcessNextAgent(ITestsConfig config, string handle)
        {
            finishedAgents.Add(new Pair<string, bool>(handle, false));
            if (Finished != config.ProcessCount) return;
            foreach (var agent in finishedAgents)
            {
                if (agent.Value) continue;
                var id = agent.Key;
                agent.Value = true;
                ThreadPool.QueueUserWorkItem(
                    o => {
                        using (var cl = new InterprocessClient<INunitRunnerServer>(id))
                        {
                            cl.Instance.CanClose();
                        }
                    });
            }
        }
    }
}
