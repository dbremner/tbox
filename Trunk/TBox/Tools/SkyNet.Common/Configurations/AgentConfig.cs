using System;
using System.Runtime.Serialization;

namespace SkyNet.Common.Configurations
{
    [DataContract]
    public class AgentConfig
    {
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string ServerEndpoint { get; set; }
        [DataMember]
        public int Port { get; set; }
        [DataMember]
        public int TotalCores { get; set; }

        public AgentConfig()
        {
            Name = Environment.MachineName;
            Port = 30001;
            ServerEndpoint = string.Format("http://localhost:{0}/", 30000);
            TotalCores = Environment.ProcessorCount;
        }
    }
}
