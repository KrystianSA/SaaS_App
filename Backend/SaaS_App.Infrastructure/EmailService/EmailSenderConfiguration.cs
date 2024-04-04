using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SaaS_App.Application.Models.Settings;
using SaaS_App.Infrastructure.Email;

namespace SaaS_App.Infrastructure.EmailService
{
    public static class EmailSenderConfiguration
    {
        public static IServiceCollection AddEmailSender(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.Configure<EmailSetting>(configuration.GetSection("MailSettings"));
            services.AddTransient<IEmailSender, EmailSender>();
            return services;
        }
    }
}
