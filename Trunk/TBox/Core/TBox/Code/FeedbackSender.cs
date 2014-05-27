using System.Net;
using System.Web;
using Mnk.Library.Common;
using Mnk.Library.Common.Log;
using Mnk.Library.Common.Network;

namespace Mnk.TBox.Core.Application.Code
{
	class FeedbackSender
	{
		private readonly ILog log = LogManager.GetLogger<FeedbackSender>();
		private readonly Request request = new Request();

		public bool Send(string title, string body)
		{
			var url = string.Format("http://mnk92.noip.me:61234/send/{0}", HttpUtility.UrlPathEncode(title));
			var result = request.GetResult(url,
			                  HttpMethod.POST,
							  string.Format("\"{0}\"", CommonEncoders.EncodeString(body)),
							  new[]
								  {
									  new Header("Content-Type", "application/json; charset=utf-8"),
									  new Header("Accept-Encoding", "gzip,deflate")
								  }
							  );
			if (result.HttpStatusCode != HttpStatusCode.OK)
			{
				log.Write("Can't send feedback, status code: " + result.HttpStatusCode);
				return false;
			}
			return true;
		}
	}
}
