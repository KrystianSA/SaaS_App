using MailKit.Net.Smtp;
using MimeKit;

namespace SaaS_App.Infrastructure.Email
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailSenderOptions _emailSenderOptions;

        public EmailSender(EmailSenderOptions emailSenderOptions)
        {
            _emailSenderOptions = emailSenderOptions;
        }
        public  void SendEmail(Message message)
        {
            var emailMessage = CreateEmailMessage(message);
            Send(emailMessage);
        }

        private MimeMessage CreateEmailMessage(Message message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("email",_emailSenderOptions.From));
            emailMessage.To.AddRange(message.To);
            emailMessage.Subject = message.Subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Text) { Text = message.Content };

            return emailMessage;
        }

        private void Send(MimeMessage mailMessage)
        {
            using (var client = new SmtpClient())
            {
                try
                {
                    client.Connect(_emailSenderOptions.SmtpServer, _emailSenderOptions.Port, true);
                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    client.Authenticate(_emailSenderOptions.UserName, _emailSenderOptions.Password);

                    client.Send(mailMessage);
                }
                catch
                {
                    //log an error message or throw an exception or both.
                    throw;
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
