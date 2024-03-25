using MailKit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace SaaS_App.Infrastructure.Email
{
    public static class EmailSenderConfiguration
    {
        public static IServiceCollection AddEmailSender(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.Configure<EmailSenderOptions>(configuration.GetSection("MailSettings"));
            services.AddTransient<IEmailSender, EmailSender>();
            return services;
        }
    }
}
