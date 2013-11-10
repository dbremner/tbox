using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace SkyNet.Common.Server
{
    [DataContract]
    public class SkyReport
    {
        [DataMember]
        public string Report { get; set; }
    }
}
