using System.Runtime.Serialization;

namespace SkyNet.Common.Agent
{
    [DataContract]
    public class AgentWork
    {
        [DataMember]
        public string Type { get; set; }

        [DataMember]
        public string CompatibleVersion { get; set; }
    }
}
