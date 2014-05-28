using System.ServiceModel;
using System.ServiceModel.Web;

namespace Mnk.TBox.Tools.SkyNet.Common
{
    [ServiceContract]
    public interface ISkyNetAgentService
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
        [WebInvoke(Method="POST", UriTemplate = "task/current/cancel", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        void CancelCurrentTask();

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "task/current/terminate", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string TerminateCurrentTask();

        [OperationContract]
        [WebInvoke(Method = "DELETE", UriTemplate = "task/{id}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string DeleteTask(string id);

        [OperationContract]
        [WebGet(UriTemplate = "ping", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        void PingIsAlive();
    }
}
