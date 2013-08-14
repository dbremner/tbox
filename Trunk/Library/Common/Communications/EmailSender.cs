using System.Net;
using System.Net.Mail;
using System.Text;

namespace Common.Communications
{
	public class EmailSender
	{
		private readonly string smtpServer;
		private readonly int port;
		private readonly string login;
		private readonly string password;

		public EmailSender(string smtpServer, int port, string login, string password)
		{
			this.smtpServer = smtpServer;
			this.port = port;
			this.login = login;
			this.password = password;
		}

		public void Send(string from, string to, string subject, string body)
		{
			using (var message = new MailMessage())
			{
				message.To.Add(to);
				message.Subject = subject;
				message.From = new MailAddress(from);
				message.BodyEncoding = Encoding.UTF8;
				message.IsBodyHtml = false;
				message.Body = body;
				using (var smtp = new SmtpClient(smtpServer, port))
				{
					smtp.EnableSsl = true;
					smtp.Credentials = new NetworkCredential(login, password);
;					smtp.Send(message);
				}
			}
		}
	}
}
