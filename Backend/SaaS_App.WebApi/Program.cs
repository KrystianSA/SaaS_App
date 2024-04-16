using Serilog;
using SaaS_App.Infrastructure.Persistence;
using SaaS_App.Infrastructure.Auth;
using SaaS_App.Application.Logic.Abstractions;
using SaaS_App.WebApi.Middlewares;
using SaaS_App.WebApi.Application;
using SaaS_App.WebApi.Application.Auth;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;
using SaaS_App.Infrastructure.EmailService;
using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;

namespace SaaS_App.WebApi
{
    public class Program
    {
        public static string APP_NAME = "SaaS.WebApi";

        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .Enrich.WithProperty("Application", APP_NAME)
                .Enrich.WithProperty("MachineName", Environment.MachineName)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateBootstrapLogger();

            var builder = WebApplication.CreateBuilder(args);

            if (builder.Environment.IsDevelopment())
            {
                builder.Configuration.AddJsonFile("appsettings.Development.local.json");
            }

            builder.Host.UseSerilog((context, services, configuration) => configuration
                .Enrich.WithProperty("Application", APP_NAME)
                .Enrich.WithProperty("MachineName", Environment.MachineName)
                .ReadFrom.Configuration(context.Configuration)
                .ReadFrom.Services(services)
                .Enrich.FromLogContext());

            // Add services to the container.
            builder.Services.AddEmailSender(builder.Configuration);
            builder.Services.AddJwtAutheticationDataProvider(builder.Configuration);
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddDatabaseCache();
            builder.Services.AddSqlDatabase(builder.Configuration.GetConnectionString("MainDbSql")!);

            builder.Services.AddControllersWithViews(options =>
            {
                if (!builder.Environment.IsDevelopment())
                {
                    options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
                }
            }).AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

            builder.Services.AddAntiforgery(options =>
            {
                options.HeaderName = "X-XSRF-TOKEN";
            });

            builder.Services.AddJwtAuth(builder.Configuration);
            builder.Services.AddPasswordManager();

            builder.Services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssemblyContaining(typeof(BaseCommandHandler));
            });
            builder.Services.AddAplicationServices();
            builder.Services.AddValidators();

            builder.Services.AddAuthentication();

            builder.Services.AddSwaggerGen(o =>
            {
                o.CustomSchemaIds(x =>
                {
                    var name = x.FullName;
                    if (name != null)
                    {
                        name = name.Replace("+", "_"); // swagger bug fix
                    }

                    return name;
                });
            });
            builder.Services.AddCors();
        
            builder.Services.AddMvc();

            builder.Services.AddRateLimiter(options =>
            {
                options.AddFixedWindowLimiter("LoginLimiter", opt =>
                {
                    opt.Window = TimeSpan.FromSeconds(60);
                    opt.PermitLimit = 3;
                });
            });

            builder.Services.AddRss();

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseCors(builder => builder
                                .WithOrigins(app.Configuration.GetValue<string>("WebAppBaseUrl") ?? "")
                                .WithOrigins(app.Configuration.GetSection("AdditionalCorsOrigins").Get<string[]>() ?? new string[0])
                                .WithOrigins((Environment.GetEnvironmentVariable("AdditionalCorsOrigins") ?? "").Split(',').Where(h => !string.IsNullOrEmpty(h)).Select(h => h.Trim()).ToArray())
                                .AllowAnyMethod()
                                .AllowCredentials()
                                .AllowAnyHeader());

            app.UseExceptionResultMiddleware();

            app.UseAuthorization();

            app.MapControllers();

            app.UseRateLimiter();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.Run();
        }
    }
}
