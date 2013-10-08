using System;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Web;
using Common.Base.Log;
using Common.Communications;

namespace FeedbackService
{
	[ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, IncludeExceptionDetailInFaults = false)]
	class FeedbackServer : IFeedbackServer
	{
		private readonly ILog log = LogManager.GetLogger<FeedbackService>();
		private readonly string fromAddress;
		private readonly string toAddress;
		private readonly EmailSender sender;
		public FeedbackServer(string smtpServer, int port, string login, string password, string fromAddress, string toAddress)
		{
			this.fromAddress = fromAddress;
			this.toAddress = toAddress;
			sender = new EmailSender(smtpServer, port, login, password);
		}

		public void Send(string subject, string body)
		{
			log.Write("Send: '{0}' # '{1}'", subject, body );
			try
			{
				sender.Send(fromAddress, toAddress, subject, body);
			}
			catch (Exception ex)
			{
				log.Write(ex, "Can't send email");
				WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.InternalServerError;
			}
		}
	}
}
