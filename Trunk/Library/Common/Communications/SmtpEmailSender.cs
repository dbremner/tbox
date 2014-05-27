using System.Net;
using System.Net.Mail;
using System.Text;

namespace Mnk.Library.Common.Communications
{
	public class SmptEmailSender : IEmailSender
	{
		private readonly string smtpServer;
		private readonly int port;
		private readonly string userName;
		private readonly string password;

		public SmptEmailSender(string smtpServer, int port, string userName, string password)
		{
			this.smtpServer = smtpServer;
			this.port = port;
			this.userName = userName;
			this.password = password;
		}

        public void Send(string subject, string body, bool isHtml, string[] recipients)
		{
			using (var message = new MailMessage())
			{
			    foreach (var email in recipients)
			    {
                    message.To.Add(email);
			    }
				message.Subject = subject;
				message.From = new MailAddress(userName);
				message.BodyEncoding = Encoding.UTF8;
				message.IsBodyHtml = isHtml;
				message.Body = body;
				using (var smtp = new SmtpClient(smtpServer, port))
				{
					smtp.EnableSsl = true;
					smtp.Credentials = new NetworkCredential(userName, password);
					smtp.Send(message);
				}
			}
		}
	}
}
