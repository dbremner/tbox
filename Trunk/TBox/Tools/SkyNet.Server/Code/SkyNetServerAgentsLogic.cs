using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Mnk.Library.Common.Communications;
using Mnk.Library.Common.Tools;
using Mnk.TBox.Tools.SkyNet.Common;
using Mnk.TBox.Tools.SkyNet.Server.Code.Interfaces;

namespace Mnk.TBox.Tools.SkyNet.Server.Code
{
    class SkyNetServerAgentsLogic : ISkyNetServerAgentsLogic
    {
        private readonly IHttpContextHelper contextHelper;
        private readonly IStorage storage;

        public SkyNetServerAgentsLogic(IHttpContextHelper contextHelper, IStorage storage)
        {
            this.contextHelper = contextHelper;
            this.storage = storage;
        }

        public IList<ServerAgent> GetAgents()
        {
            return storage.Config.Agents;
        }

        public void ConnectAgent(ServerAgent agent)
        {
            if (string.IsNullOrEmpty(agent.Endpoint) || agent.TotalCores <= 0)
            {
                contextHelper.SetStatusCode(HttpStatusCode.ExpectationFailed);
                return;
            }
            var exist = storage.Config.Agents.FirstOrDefault(x => x.Endpoint.EqualsIgnoreCase(agent.Endpoint));
            if (exist != null)
            {
                if(exist.TotalCores == agent.TotalCores)return;
                exist.TotalCores = agent.TotalCores;
            }
            else
            {
                storage.Config.Agents.Add(new ServerAgent
                {
                    Endpoint = agent.Endpoint,
                    TotalCores = agent.TotalCores,
                    State = AgentState.Idle
                });
            }
            storage.Save();
        }

        public void DisconnectAgent(string endpoint)
        {
            var agent = storage.Config.Agents.FirstOrDefault(x => x.Endpoint.EqualsIgnoreCase(endpoint));
            if (agent == null)
            {
                contextHelper.SetStatusCode(HttpStatusCode.NotFound);
                return;
            }
            storage.Config.Agents.Remove(agent);
            storage.Save();
        }

        public void PingIsAlive()
        {
        }
    }
}
