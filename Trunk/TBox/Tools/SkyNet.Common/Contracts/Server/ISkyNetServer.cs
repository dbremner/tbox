using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace Mnk.TBox.Tools.SkyNet.Common.Contracts.Server
{
    [ServiceContract]
    public interface ISkyNetServer
    {
        [OperationContract]
        [WebGet(UriTemplate = "agent/all", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        IList<ServerAgent> GetAgents();

        [OperationContract]
        [WebInvoke(Method = "CREATE", UriTemplate = "agent", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        void ConnectAgent(ServerAgent agent);

        [OperationContract]
        [WebInvoke(Method = "DELETE", UriTemplate = "agent", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        void DisconnectAgent(string endpoint);

        [OperationContract]
        [WebInvoke(Method = "CREATE", UriTemplate = "task", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string AddTask(ServerTask task);

        [OperationContract]
        [WebGet(UriTemplate = "task/all", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        IList<ServerTask> GetTasks();

        [OperationContract]
        [WebInvoke(Method = "DELETE", UriTemplate = "task/{id}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string DeleteTask(string id);

        [OperationContract]
        [WebGet(UriTemplate = "ping", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        void PingIsAlive();

    }
}
