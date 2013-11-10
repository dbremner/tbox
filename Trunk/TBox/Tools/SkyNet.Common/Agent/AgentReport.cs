using System.Runtime.Serialization;

namespace SkyNet.Common.Agent
{
    [DataContract]
    public class AgentReport
    {
        [DataMember]
        public string Report { get; set; }
    }
}
