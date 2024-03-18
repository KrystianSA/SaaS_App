using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using SaaS_App.Application.Interfaces;
using SaaS_App.Application.Logic.Abstractions;
using SaaS_App.Application.Services;
using SaaS_App.Application.Validators;

namespace SaaS_App.WebApi.Application
{
    public static class DefaultDIConfiguration
    {
        public static IServiceCollection AddAplicationServices(this IServiceCollection services)
        {
            services.AddScoped<ICurrentAccountProvider, CurrentAccountProvider>();
            return services;
        }
        public static IServiceCollection AddValidators(this IServiceCollection services)
        {
            services.AddValidatorsFromAssemblyContaining(typeof(BaseQueryHandler));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            return services;
        }
    }
}
