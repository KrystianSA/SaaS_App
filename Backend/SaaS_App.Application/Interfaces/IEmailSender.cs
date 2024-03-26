namespace SaaS_App.Infrastructure.Email
{
    public interface IEmailSender
    {
        bool SendEmail(Message message);
    }
}
