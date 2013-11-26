using System;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Web;
using Common.Base.Log;
using Common.Tools;
using SkyNet.Common.Configurations;
using SkyNet.Common.Contracts.Agent;

namespace SkyNet.Agent
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, IncludeExceptionDetailInFaults = false)]
    public class SkyNetAgent : ISkyNetAgent
    {
        private readonly AgentConfig config;
        private readonly ILog log = LogManager.GetLogger<SkyNetAgent>();
        private readonly object locker = new object();
        private AgentTask currentTask = null;

        public SkyNetAgent(AgentConfig config)
        {
            this.config = config;
        }

        public string AddTask(AgentTask task)
        {
            try
            {
                lock (locker)
                {
                    currentTask = task;
                    return currentTask.Id = Guid.NewGuid().ToString();
                }
            }
            catch (Exception ex)
            {
                ProcessError(ex);
                return string.Empty;
            }
        }

        public AgentTask GetTask(string id)
        {
            try
            {
                lock (locker)
                {
                    if (currentTask == null || !currentTask.Id.EqualsIgnoreCase(id))
                    {
                        SetStatusCode(HttpStatusCode.NotFound);
                        return null;
                    }
                    return currentTask;
                }
            }
            catch (Exception ex)
            {
                ProcessError(ex);
                return null;
            }
        }

        public AgentTask GetCurrentTask()
        {
            try
            {
                lock (locker)
                {
                    if (currentTask == null) return null;
                    return new AgentTask
                    {
                        Progress = currentTask.Progress,
                        IsDone = currentTask.IsDone,
                    };
                }
            }
            catch (Exception ex)
            {
                ProcessError(ex);
                return null;
            }
        }

        public string DeleteTask(string id)
        {
            try
            {
                lock (locker)
                {
                    if (currentTask == null || !currentTask.Id.EqualsIgnoreCase(id))
                    {
                        SetStatusCode(HttpStatusCode.NotFound);
                        return string.Empty;
                    }
                    var report = currentTask.Report;
                    currentTask = null;
                    return report;
                }
            }
            catch (Exception ex)
            {
                ProcessError(ex);
                return string.Empty;
            }
        }

        public void PingIsAlive()
        {
        }

        private static void SetStatusCode(HttpStatusCode status)
        {
            if (WebOperationContext.Current != null)
            {
                WebOperationContext.Current.OutgoingResponse.StatusCode = status;
            }
        }

        private void ProcessError(Exception ex)
        {
            log.Write(ex, "Unexpected exception");
            SetStatusCode(HttpStatusCode.InternalServerError);
        }

    }
}
