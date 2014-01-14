using System.Runtime.Serialization;

namespace Mnk.TBox.Tools.SkyNet.Common.Contracts.Server
{
    [DataContract]
    public class ServerAgent
    {
        [DataMember]
        public string Endpoint { get; set; }

        [DataMember]
        public int TotalCores { get; set; }

        [DataMember]
        public ServerAgentState State { get; set; }
    }
}
