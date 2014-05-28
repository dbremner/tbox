using System;
using Mnk.Library.Common.Log;
using Mnk.TBox.Tools.SkyNet.Common;
using Mnk.TBox.Tools.SkyNet.Server.Code.Interfaces;

namespace Mnk.TBox.Tools.SkyNet.Server.Code.Processing
{
    class SkyAgentLogic : ISkyAgentLogic
    {
        private readonly IAgentsCache cache;
        private readonly ILog log = LogManager.GetLogger<SkyAgentLogic>();

        public SkyAgentLogic(IAgentsCache cache)
        {
            this.cache = cache;
        }

        public WorkerTask CreateWorkerTask(ServerAgent agent, string agentData, ServerTask task)
        {
            var agentTask = new AgentTask
            {
                Config = agentData,
                Script = task.Script,
                ZipPackageId = task.ZipPackageId
            };
            try
            {
                task.Id = cache.Get(agent).AddTask(agentTask);
                return new WorkerTask
                {
                    Agent = agent,
                    Task = agentTask
                };
            }
            catch (Exception ex)
            {
                log.Write(ex, "Can't create agent task");
                return new WorkerTask
                {
                    Exception = ex
                };
            }
        }

        public bool IsDone(WorkerTask task)
        {
            if (task.IsFailed) return true;
            var result = true;
            Execute(task.Agent, s => result = s.GetTask(task.Task.Id).IsDone);
            return result;
        }

        public SkyAgentWork BuildReport(WorkerTask task)
        {
            var report = string.Empty;
            if(!task.IsFailed)
            {
                try
                {
                    report = cache.Get(task.Agent).DeleteTask(task.Task.Id);
                }
                catch (Exception ex)
                {
                    log.Write(ex, "Can't get agent task state");
                    task.Exception = ex;
                }
            }
            if (task.IsFailed)
            {
                report = task.Exception.ToString();
            }
            return new SkyAgentWork
            {
                Config = task.Config,
                Agent = task.Agent,
                Report = report
            };
        }

        public bool IsAlive(ServerAgent agent)
        {
            var result = false;
            Execute(agent, s => { 
                s.PingIsAlive();
                result = true;
            });
            return result;
        }

        public string TerminateCurrentTask(ServerAgent agent)
        {
            var result = string.Empty;
            Execute(agent, s => result = s.TerminateCurrentTask());
            return result;
        }

        public void DeleteTask(ServerAgent agent, string id)
        {
            Execute(agent, s=>s.DeleteTask(id));
        }

        private void Execute(ServerAgent agent, Action<ISkyNetAgentService> op)
        {
            try
            {
                op(cache.Get(agent));
            }
            catch (Exception ex)
            {
                log.Write(ex, "Unexpected error for agent: " + agent.Endpoint);
            }
        }
    }
}
