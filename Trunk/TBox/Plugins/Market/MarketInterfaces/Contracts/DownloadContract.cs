using System.ServiceModel;

namespace MarketInterfaces.Contracts
{
	[MessageContract]
	public class DownloadContract
	{
		[MessageHeader(MustUnderstand = true)]
		public string Author;

		[MessageHeader(MustUnderstand = true)]
		public string Name;
	}
}
