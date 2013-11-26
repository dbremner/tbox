using System.ServiceModel;

namespace MarketInterfaces.Contracts
{
	[MessageContract]
	public class PluginUploadContract : DataContract
	{
		[MessageHeader(MustUnderstand = true)]
		public Plugin Item;
	}
}
