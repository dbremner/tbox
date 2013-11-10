using System.Runtime.Serialization;

namespace SkyNet.Common.Server
{
    [DataContract]
    public class SkyWork
    {
        [DataMember]
        public string Id { get; set; }

        [DataMember]
        public string Type { get; set; }

        [DataMember]
        public string Config { get; set; }

        [DataMember]
        public string CompatibleVersion { get; set; }
    }
}
