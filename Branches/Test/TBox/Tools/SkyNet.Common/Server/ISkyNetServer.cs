using System.ServiceModel;
using System.ServiceModel.Web;

namespace SkyNet.Common.Server
{
    [ServiceContract]
    public interface ISkyNetServer
    {
        [OperationContract]
        [WebGet(UriTemplate = "works", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        SkyWork[] GetWorks();

        [OperationContract]
        [WebGet(UriTemplate = "agents", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        SkyAgent[] GetAgents();

        [OperationContract]
        [WebInvoke(Method = "CREATE", UriTemplate = "task", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string AddTask(SkyTask task);

        [OperationContract]
        [WebGet(UriTemplate = "task/{id}/state", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        SkyState GetState(string id);

        [OperationContract]
        [WebGet(UriTemplate = "task/{id}/report", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        SkyReport GetReport(string id);

        [OperationContract]
        [WebInvoke(Method = "DELETE", UriTemplate = "task/{id}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        void DeleteTask(string id);

        [OperationContract]
        [WebInvoke(Method = "CREATE", UriTemplate = "agents", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        void AddAgent(string endpoint);

        [OperationContract]
        [WebInvoke(Method = "DELETE", UriTemplate = "agents", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        void DeleteAgent(string endpoint);
    }
}
