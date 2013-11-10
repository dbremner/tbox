using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace SkyNet.Common.Server
{
    [DataContract]
    public class SkyTask
    {
        [DataMember]
        public string Id { get; set; }

        [DataMember]
        public string Config { get; set; }
    }
}
