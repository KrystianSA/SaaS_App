using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SaaS_App.Application.Interfaces;
using SaaS_App.Infrastructure.Rss;

namespace SaaS_App.Infrastructure.Auth
{
    public static class RssConfiguration
    {
        public static IServiceCollection AddRss(this IServiceCollection services)
        {
            services.AddScoped<IRssManager, RssManager>();
            return services;
        }
    }
}
