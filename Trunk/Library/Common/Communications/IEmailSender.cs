namespace Mnk.Library.Common.Communications
{
    public interface IEmailSender
    {
        void Send(string subject, string body, bool isHtml, string[] recipients);
    }
}
