using Authenticity.CourtSide.Core.Models;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using System;
using System.Net.Mail;

namespace Authenticity.CourtSide.Core.Helpers
{
    static class EmailHelper
    {
        public static void SendEmail(EmailConfiguration emailConfig, string to, string body, string subject)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(emailConfig.From, emailConfig.From));
            message.To.Add(new MailboxAddress(to, to));
            message.Subject = subject;
            var bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = body;

            message.Body = bodyBuilder.ToMessageBody();

            using (var client = new MailKit.Net.Smtp.SmtpClient())
            {
                client.Connect(emailConfig.SmtpServer, emailConfig.Port, SecureSocketOptions.StartTls);

                client.Authenticate(emailConfig.UserName, emailConfig.Password);

                client.Send(message);
                client.Disconnect(true);
            }
        }

        public static void SendEmailOffice365(EmailConfiguration emailConfig, string to, string body, string subject)
        {
            var message = string.Empty;
            MailMessage msg = new MailMessage();
            msg.To.Add(new MailAddress(to, to));
            msg.From = new MailAddress(emailConfig.From, emailConfig.From);
            msg.Subject = subject;
            msg.Body = body;
            msg.IsBodyHtml = true;
            System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient();
            client.UseDefaultCredentials = false;
            client.Credentials = new System.Net.NetworkCredential(emailConfig.UserName, emailConfig.Password);
			client.Port = emailConfig.Port;
            client.Host = emailConfig.SmtpServer;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.EnableSsl = true;
            try
            {
                client.Send(msg);
            }
            catch (Exception ex)
            {
                message = ex.ToString();
            }
            finally 
            {
                client.Dispose();
            }
        }
    }
}
