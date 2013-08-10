using System.Net.Mail;
using System.Text;

namespace Common.Network
{
	public class EmailSender
	{
		private readonly string smtpServer;
		private readonly int port;

		public EmailSender(string smtpServer, int port)
		{
			this.smtpServer = smtpServer;
			this.port = port;
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
					smtp.UseDefaultCredentials = false;
					smtp.EnableSsl = false;
					smtp.Credentials = null;
;					smtp.Send(message);
				}
			}
		}
	}
}
