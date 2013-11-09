using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace SkyNet.Common.Agent
{
    [DataContract]
    public class AgentCore
    {
        [DataMember]
        public bool IsIdle { get; set; }

        [DataMember]
        public int Progress { get; set; }
    }
}
