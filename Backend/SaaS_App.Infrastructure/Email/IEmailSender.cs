namespace SaaS_App.Infrastructure.Email
{
    public interface IEmailSender
    {
        void SendEmail(Message message);
    }
}
