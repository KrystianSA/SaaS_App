using SaaS_App.Application.Interfaces;

namespace SaaS_App.WebApi.Application.Auth
{
    public static class JwtAutheticationDataProviderConfiguration
    {
        public static IServiceCollection AddJwtAutheticationDataProvider(this IServiceCollection services, 
                IConfiguration configuration)
        {
            services.Configure<CookieSettings>(configuration.GetSection("CookieSettings"));
            services.AddScoped<IAuthenticationDataProvider, JwtAutheticationDataProvider>();
            return services;    
        }
    }
}
