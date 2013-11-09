using System;
using System.Runtime.Serialization;

namespace MarketInterfaces
{
	[DataContract]
	public class Bug
	{
		[DataMember]public long UID;
		[DataMember]public long PluginUID;
		[DataMember]public string Description;
		[DataMember]public DateTime Date;
	}
}
