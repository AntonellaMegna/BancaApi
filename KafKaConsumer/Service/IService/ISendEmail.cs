namespace KafKaConsumer.Service.IService
{
    public interface ISendEmail
    {
        void SendEmailToAll(string toAddress, string fromAddress, string subject, string messageText, bool isHtmlMessage, string username, string password, string smtp, int port);
    }
}