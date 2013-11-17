﻿using System.ServiceModel;
using System.ServiceModel.Web;

namespace SkyNet.Common.Contracts.Agent
{
    [ServiceContract]
    public interface ISkyNetAgent
    {
        [OperationContract]
        [WebInvoke(Method = "CREATE", UriTemplate = "task", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string AddTask(AgentTask task);

        [OperationContract]
        [WebGet(UriTemplate = "task/{id}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        AgentTask GetTask(string id);

        [OperationContract]
        [WebGet(UriTemplate = "task/current", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        AgentTask GetCurrentTask();

        [OperationContract]
        [WebInvoke(Method = "DELETE", UriTemplate = "task/{id}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string DeleteTask(string id);

        [OperationContract]
        [WebGet(UriTemplate = "ping", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        void PingIsAlive();
    }
}
