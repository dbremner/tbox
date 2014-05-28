using System;
using System.Net;
using Mnk.Library.Common.Communications;
using Mnk.Library.Common.Tools;
using Mnk.TBox.Tools.SkyNet.Agent.Code.Interfaces;
using Mnk.TBox.Tools.SkyNet.Common;

namespace Mnk.TBox.Tools.SkyNet.Agent.Code
{
    class SkyNetAgentLogic : ISkyNetAgentLogic
    {
        private AgentTask currentTask = null;
        private readonly IWorker worker;
        private readonly IHttpContextHelper contextHelper;


        public SkyNetAgentLogic(IWorker worker, IHttpContextHelper contextHelper)
        {
            this.worker = worker;
            this.contextHelper = contextHelper;
        }

        public string AddTask(AgentTask task)
        {
            if (currentTask != null)
            {
                contextHelper.SetStatusCode(HttpStatusCode.Conflict);
                return string.Empty;
            }
            currentTask = task;
            currentTask.Progress = 0;
            currentTask.IsDone = false;
            currentTask.Report = string.Empty;
            worker.Start(currentTask);
            return currentTask.Id = Guid.NewGuid().ToString();

        }

        public AgentTask GetTask(string id)
        {
            if (currentTask == null || !currentTask.Id.EqualsIgnoreCase(id))
            {
                contextHelper.SetStatusCode(HttpStatusCode.NotFound);
                return null;
            }
            return currentTask;
        }

        public AgentTask GetCurrentTask()
        {
            if (currentTask == null) return null;
            return new AgentTask
            {
                Progress = currentTask.Progress,
                IsDone = worker.IsDone || currentTask.IsCanceled,
            };
        }

        public void CancelCurrentTask()
        {
            if (currentTask == null) return;
            currentTask.IsCanceled = true;
            worker.Cancel();
        }

        public string TerminateCurrentTask()
        {
            if (currentTask == null) return string.Empty;
            currentTask.IsCanceled = true;
            worker.Terminate();
            return currentTask.Id;
        }

        public string DeleteTask(string id)
        {
            if (currentTask == null || !currentTask.Id.EqualsIgnoreCase(id))
            {
                contextHelper.SetStatusCode(HttpStatusCode.NotFound);
                return string.Empty;
            }
            if (!(worker.IsDone || currentTask.IsCanceled))
            {
                contextHelper.SetStatusCode(HttpStatusCode.Conflict);
                return string.Empty;
            }
            var report = currentTask.Report;
            currentTask = null;
            return report;
        }

        public void PingIsAlive()
        {
        }
    }
}
