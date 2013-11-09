using System.Runtime.Serialization;

namespace SkyNet.Common.Agent
{
    [DataContract]
    public class AgentState
    {
        [DataMember]
        public int Progress { get; set; }

        [DataMember]
        public bool IsIdle { get; set; }
    }
}
