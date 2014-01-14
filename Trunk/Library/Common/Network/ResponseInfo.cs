using System.Net;

namespace Mnk.Library.Common.Network
{
	public class ResponseInfo
	{
		public string Body { get; set; }
		public string Headers { get; set; }
		public HttpStatusCode HttpStatusCode { get; set; }
		public long Time { get; set; }

	}
}
