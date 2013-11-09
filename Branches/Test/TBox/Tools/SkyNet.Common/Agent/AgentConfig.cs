using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace SkyNet.Common.Agent
{
    [DataContract]
    public class AgentConfig
    {
        [DataMember]
        public int MaxCores { get; set; }

        [DataMember]
        public string ServerEndpoint { get; set; }
    }
}
