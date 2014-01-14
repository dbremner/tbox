using System.Collections.Generic;
using System.Threading;
using Mnk.Library.Common.Communications.Interprocess;
using Mnk.Library.Common.Data;
using Mnk.Library.ParallelNUnit.Interfaces;

namespace Mnk.Library.ParallelNUnit.Infrastructure
{
    public class Synchronizer
    {
        private readonly int expectedAgentsCount;
        private readonly IList<Pair<string, bool>> finishedAgents = new List<Pair<string, bool>>();

        public int Count { get { return expectedAgentsCount; } }
        public int Finished { get { return finishedAgents.Count; } }

        public Synchronizer(int expectedAgentsCount)
        {
            this.expectedAgentsCount = expectedAgentsCount;
        }

        public void ProcessNextAgent(string handle)
        {
            finishedAgents.Add(new Pair<string, bool>(handle, false));
            if (Finished != Count) return;
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
