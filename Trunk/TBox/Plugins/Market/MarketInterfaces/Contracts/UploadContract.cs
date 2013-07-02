using System.ServiceModel;

namespace MarketInterfaces.Contracts
{
	[MessageContract]
	public class UploadContract
	{
		[MessageHeader(MustUnderstand = true)]
		public bool Success;

		[MessageHeader(MustUnderstand = true)]
		public bool Exist;
	}
}
