using System;
using System.Collections.Generic;
using System.IO;
using Mnk.Library.Common.Communications;
using Mnk.Library.Common.Tools;
using Mnk.TBox.Plugins.SkyNet.Code.Interfaces;
using Mnk.TBox.Tools.SkyNet.Common;

namespace Mnk.TBox.Plugins.SkyNet.Code
{
    class ServicesFacade : IServicesFacade
    {
        public IList<ServerTask> GetServiceTasks(AgentConfig config)
        {
            using (var serverClient = CreateServerTasksClient(config))
            {
                return serverClient.Instance.GetTasks();
            }
        }

        public IList<ServerAgent> GetServiceAgents(AgentConfig config)
        {
            using (var serverClient = CreateServerAgentsClient(config))
            {
                return serverClient.Instance.GetAgents();
            }
        }

        public AgentTask GetAgentCurrentTask(AgentConfig config)
        {
            using (var agentClient = CreateAgentServerClient(config))
            {
                return agentClient.Instance.GetCurrentTask();
            }
        }

        public string UploadFile(AgentConfig config, string path)
        {
            using (var cl = CreateFileServerClient(config))
            {
                using (var f = File.OpenRead(path))
                {
                    return cl.Instance.Upload(f);
                }
            }
        }

        public string StartTask(AgentConfig config, ServerTask task)
        {
            using (var cl = CreateServerTasksClient(config))
            {
                return cl.Instance.AddTask(task);
            }
        }

        private NetworkClient<ISkyNetServerAgentsService> CreateServerAgentsClient(AgentConfig config)
        {
            return new NetworkClient<ISkyNetServerAgentsService>(new Uri(config.ServerEndpoint));
        }

        private NetworkClient<ISkyNetServerTasksService> CreateServerTasksClient(AgentConfig config)
        {
            return new NetworkClient<ISkyNetServerTasksService>(new Uri(config.ServerEndpoint));
        }

        private NetworkClient<ISkyNetFileService> CreateFileServerClient(AgentConfig config)
        {
            return new NetworkClient<ISkyNetFileService>(new Uri(config.ServerEndpoint));
        }

        private NetworkClient<ISkyNetAgentService> CreateAgentServerClient(AgentConfig config)
        {
            return new NetworkClient<ISkyNetAgentService>(Environment.MachineName, config.Port);
        }
    }
}
