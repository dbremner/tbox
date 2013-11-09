using System;
using System.ServiceModel;
using SkyNet.Common.Agent;

namespace SkyNet.Agent
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, IncludeExceptionDetailInFaults = false)]
    public class SkyNetAgent : ISkyNetAgent
    {
        public AgentWork[] GetWorks()
        {
            throw new NotImplementedException();
        }

        public AgentCore[] GetCores()
        {
            throw new NotImplementedException();
        }

        public string AddTask(AgentTask task)
        {
            throw new NotImplementedException();
        }

        public AgentState GetState(string id)
        {
            throw new NotImplementedException();
        }

        public AgentReport GetReport(string id)
        {
            throw new NotImplementedException();
        }

        public void DeleteTask(string id)
        {
            throw new NotImplementedException();
        }

        public void ClearTasks()
        {
            throw new NotImplementedException();
        }
    }
}
