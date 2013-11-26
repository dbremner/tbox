namespace Common.Communications
{
	public interface IEmailSender
	{
        void Send(string subject, string body, bool isHtml, string[] to);
	}
}
