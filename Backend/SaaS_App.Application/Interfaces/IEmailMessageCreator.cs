using SaaS_App.Application.Models.Email;

namespace SaaS_App.Application.Interfaces
{
    public interface IEmailMessageCreator
    {
        EmailMessage CreateEmailWithUrl(string email, string subject, string websiteName, Dictionary<string, string> parametersToUrl, string text = null); 
    }
}
