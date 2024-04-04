using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using SaaS_App.Application.Models.Email;
using SaaS_App.Application.Models.Settings;
using SaaS_App.Infrastructure.Email;

namespace SaaS_App.Infrastructure.EmailService
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailSetting _options;

        public EmailSender(IOptions<EmailSetting> options)
        {
            _options = options.Value;
        }
        public bool SendEmail(EmailMessage message)
        {
            var emailMessage = CreateEmailMessage(message);
            var isSent = Send(emailMessage);
            return isSent;
           
        }

        private MimeMessage CreateEmailMessage(EmailMessage message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("Sender", _options.From));
            emailMessage.To.Add(new MailboxAddress("Audience",message.To));
            emailMessage.Subject = message.Subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Text) { Text = message.Content };

            return emailMessage;
        }

        private bool Send(MimeMessage mailMessage)
        {
            using (var client = new SmtpClient())
            {
                try
                {
                    client.Connect(_options.SmtpServer, _options.Port, true);
                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    client.Authenticate(_options.UserName, _options.Password);
                    client.Send(mailMessage);
                    return true;
                }
                catch
                {
                    //log an error message or throw an exception or both.
                    //throw;
                    return false;
                }
                finally
                {
                    client.Disconnect(true);
                    client.Dispose();
                }
            }
        }
    }
}
