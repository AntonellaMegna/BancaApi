using Confluent.Kafka;
using KafKaConsumer.Service.IService;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Text;

namespace KafKaConsumer.Service
{

    public class SendEmail : ISendEmail
    {

        //public void SendEmailToAll(string toAddress, string fromAddress, string subject, string messageText, bool isHtmlMessage, string username, string password,string smtp,int port)
        //{
        //    MailAddress from = new MailAddress(fromAddress);

        //    using (SmtpClient client = new SmtpClient(smtp, port) { DeliveryMethod = SmtpDeliveryMethod.Network, EnableSsl = true, UseDefaultCredentials = false })
        //    {
        //        client.Credentials = new System.Net.NetworkCredential(username, password);

        //        MailAddress to = new MailAddress(toAddress);

        //        using (MailMessage message = new MailMessage(from, to) { IsBodyHtml = isHtmlMessage })
        //        {
        //            message.Subject = subject;
        //            message.Body = messageText;
        //            client.EnableSsl = true;
        //            client.UseDefaultCredentials = false;
        //            client.Send(message);
        //        }
        //    }
        //}
        public void SendEmailToAll(string toAddress, string fromAddress, string subject, string messageText, bool isHtmlMessage, string username, string password, string smtp, int port)
        {
            SmtpClient client = new SmtpClient(smtp, port);
            client.EnableSsl = true;

            client.Credentials = new NetworkCredential(username, password);
            MailMessage message = new MailMessage();
            message.From = new MailAddress(fromAddress);
            message.To.Add(toAddress);
            message.Subject = subject;
            message.Body = messageText;

            try
            {
                client.Send(message);
                Console.WriteLine("Email successfully sent!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error sending email: " + ex.Message);
            }
        }
    }
}
