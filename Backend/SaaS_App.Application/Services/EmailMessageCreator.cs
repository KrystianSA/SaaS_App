using Microsoft.Extensions.Configuration;
using SaaS_App.Application.Interfaces;
using SaaS_App.Application.Models.Email;

namespace SaaS_App.Application.Services
{
    public class EmailMessageCreator : IEmailMessageCreator
    {
        private readonly string _domainName;

        public EmailMessageCreator(IConfiguration configuration)
        {
            _domainName = configuration["WebAppBaseUrl"]!;
        }
        public EmailMessage CreateEmailWithUrl(string email,string subject,string websiteName, Dictionary<string, string> parameters)
        {
            var message = new EmailMessage()
            {
                To = email,
                Subject = subject,
                Content = ""
            };
            message.Content = GenerateUrl(_domainName, websiteName, parameters);
            return message;
        }
        private string GenerateUrl(string domainName, string site, Dictionary<string, string> parameters)
        {
            return domainName + "/" + site + "?" + string.Join("&", parameters.Select(x => $"{x.Key}={x.Value}"));
        }
    }
}
