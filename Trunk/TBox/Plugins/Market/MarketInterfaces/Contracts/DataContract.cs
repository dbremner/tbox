using System;
using System.IO;
using System.ServiceModel;
using Common.Data;

namespace MarketInterfaces.Contracts
{
	[MessageContract]
	public class DataContract : IDisposable
	{
		[MessageHeader]
		public Pair<string, int>[] Descriptions;

		[MessageHeader(MustUnderstand = true)]
		public long Length;

		[MessageBodyMember(Order = 1)]
		public Stream FileByteStream;
		public void Dispose()
		{
			if (FileByteStream == null) return;
			FileByteStream.Close();
			FileByteStream = null;
		}
	}
}
