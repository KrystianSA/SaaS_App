using MimeKit;

namespace SaaS_App.Infrastructure.Email
{
    public class Message
    {
        public List<MailboxAddress> To { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public Message(List<MailboxAddress> to, string subject, string content)
        {
            To = to;
            Subject = subject;
            Content = content;
        }
    }
}
