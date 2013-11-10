using System.Runtime.Serialization;

namespace SkyNet.Common.Server
{
    [DataContract]
    public class SkyState
    {
        [DataMember]
        public int Progress { get; set; }

        [DataMember]
        public bool IsIdle { get; set; }
    }
}
