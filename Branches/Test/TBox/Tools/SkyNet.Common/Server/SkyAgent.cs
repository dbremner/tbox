using System.Runtime.Serialization;

namespace SkyNet.Common.Server
{
    [DataContract]
    public class SkyAgent
    {
        [DataMember]
        public string Id { get; set; }

        [DataMember]
        public int TotalCores { get; set; }

        [DataMember]
        public int FreeCores { get; set; }

        [DataMember]
        public int Progress { get; set; }
    }
}
