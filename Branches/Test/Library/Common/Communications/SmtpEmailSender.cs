using System.Net;
using System.Net.Mail;
using System.Text;

namespace Common.Communications
{
	public class SmptEmailSender : IEmailSender
	{
		private readonly string smtpServer;
		private readonly int port;
		private readonly string login;
		private readonly string password;

		public SmptEmailSender(string smtpServer, int port, string login, string password)
		{
			this.smtpServer = smtpServer;
			this.port = port;
			this.login = login;
			this.password = password;
		}

        public void Send(string subject, string body, bool isHtml, string[] to)
		{
			using (var message = new MailMessage())
			{
			    foreach (var email in to)
			    {
                    message.To.Add(email);
			    }
				message.Subject = subject;
				message.From = new MailAddress(login);
				message.BodyEncoding = Encoding.UTF8;
				message.IsBodyHtml = isHtml;
				message.Body = body;
				using (var smtp = new SmtpClient(smtpServer, port))
				{
					smtp.EnableSsl = true;
					smtp.Credentials = new NetworkCredential(login, password);
					smtp.Send(message);
				}
			}
		}
	}
}
