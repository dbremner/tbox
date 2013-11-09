using System;
using System.ServiceModel;
using SkyNet.Common.Server;

namespace SkyNet.Server
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, IncludeExceptionDetailInFaults = false)]
    public class SkyNetServer : ISkyNetServer
    {
        public SkyWork[] GetWorks()
        {
            throw new NotImplementedException();
        }

        public SkyAgent[] GetAgents()
        {
            throw new NotImplementedException();
        }

        public string AddTask(SkyTask task)
        {
            throw new NotImplementedException();
        }

        public SkyState GetState(string id)
        {
            throw new NotImplementedException();
        }

        public SkyReport GetReport(string id)
        {
            throw new NotImplementedException();
        }

        public void DeleteTask(string id)
        {
            throw new NotImplementedException();
        }

        public void AddAgent(string endpoint)
        {
            throw new NotImplementedException();
        }

        public void DeleteAgent(string endpoint)
        {
            throw new NotImplementedException();
        }
    }
}
