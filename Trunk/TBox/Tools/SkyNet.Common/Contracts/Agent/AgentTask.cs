﻿using System.Runtime.Serialization;

namespace SkyNet.Common.Contracts.Agent
{
    [DataContract]
    public class AgentTask
    {
        [DataMember]
        public string Id { get; set; }

        [DataMember]
        public string Script { get; set; }

        [DataMember]
        public string Config { get; set; }

        [DataMember]
        public int Progress { get; set; }

        [DataMember]
        public bool IsDone { get; set; }

        [DataMember]
        public string Report { get; set; }

    }
}
