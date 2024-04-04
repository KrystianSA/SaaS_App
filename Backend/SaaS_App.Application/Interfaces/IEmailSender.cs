using SaaS_App.Application.Models.Email;

namespace SaaS_App.Infrastructure.Email
{
    public interface IEmailSender
    {
        bool SendEmail(EmailMessage message);
    }
}
