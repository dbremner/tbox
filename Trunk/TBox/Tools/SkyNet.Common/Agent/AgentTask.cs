using System.Runtime.Serialization;

namespace SkyNet.Common.Agent
{
    [DataContract]
    public class AgentTask
    {
        [DataMember]
        public string Type { get; set; }

        [DataMember]
        public int Count { get; set; }

        [DataMember]
        public string Config { get; set; }
    }
}
