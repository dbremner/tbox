using System.ServiceModel;
using System.ServiceModel.Web;

namespace SkyNet.Common.Agent
{
    [ServiceContract]
    public interface ISkyNetAgent
    {
        [OperationContract]
        [WebGet(UriTemplate = "works", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        AgentWork[] GetWorks();

        [OperationContract]
        [WebGet(UriTemplate = "cores", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        AgentCore[] GetCores();

        [OperationContract]
        [WebInvoke(Method = "CREATE", UriTemplate = "task", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string AddTask(AgentTask task);

        [OperationContract]
        [WebGet(UriTemplate = "task/{id}/state", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        AgentState GetState(string id);

        [OperationContract]
        [WebGet(UriTemplate = "task/{id}/report", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        AgentReport GetReport(string id);

        [OperationContract]
        [WebInvoke(Method = "DELETE", UriTemplate = "task/{id}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        void DeleteTask(string id);

        [OperationContract]
        [WebInvoke(Method = "DELETE", UriTemplate = "task", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        void ClearTasks();
    }
}
