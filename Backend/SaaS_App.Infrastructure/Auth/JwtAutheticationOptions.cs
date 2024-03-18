using Microsoft.Extensions.DependencyInjection;

namespace SaaS_App.Infrastructure.Auth
{
    public class JwtAutheticationOptions
    {
        public string? Secret { get; set; }
        public string? Issuer { get; set; }
        public string? Audience { get; set; }
        public int ExpireInDays { get; set; } = 30;
    }
}
