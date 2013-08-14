using System.Net;
using System.Web;
using Common.Base.Log;
using Common.Encoders;
using Common.Network;

namespace TBox.Code
{
	class FeedbackSender
	{
		private readonly ILog log = LogManager.GetLogger<FeedbackSender>();
		private readonly Request request = new Request();

		public void Send(string title, string body)
		{
			var url = string.Format("http://mnk.dyndns-pics.com:61234/send/{0}", HttpUtility.UrlPathEncode(title));
			var result = request.GetResult(url,
			                  Methods.POST,
							  string.Format("\"{0}\"", CommonOps.EncodeString(body)),
							  new[]
								  {
									  new Header("Content-Type", "application/json; charset=utf-8"),
									  new Header("Accept-Encoding", "gzip,deflate")
								  }
							  );
			if (result.HttpStatusCode != HttpStatusCode.OK)
			{
				log.Write("Can't send feedback, status code: " + result.HttpStatusCode);
			}
		}
	}
}
