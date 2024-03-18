using SaaS_App.Application.Exceptions;
using SaaS_App.WebApi.Application.Response;
using System.Net;

namespace SaaS_App.WebApi.Middlewares
{
    public class ExceptionResultMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionResultMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task Invoke(HttpContext context, ILogger<ExceptionResultMiddleware> logger)
        {
            try
            {
                await _next(context);
            }
            catch (ErrorException ex)
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                await context.Response.WriteAsJsonAsync(new ErrorResponse { Error = ex._error });
            }
            catch (ValidationException ve)
            {
                context.Response.StatusCode = (int)HttpStatusCode.UnprocessableEntity;
                await context.Response.WriteAsJsonAsync(new ValidationErrorResponse(ve));
            }
            catch (UnauthorizedException ue)
            {
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                await context.Response.WriteAsJsonAsync(new UnauthorizedResponse { Reason = ue.Message ?? "Unauthorized" });
            }
            catch (Exception e)
            {
                logger.LogCritical(e, "Fatal error");
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                await context.Response.WriteAsJsonAsync("Server Error");
            }
        }
    }
    public static class ExceptionResultMiddlewareExtension
    {
        public static IApplicationBuilder UseExceptionResultMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionResultMiddleware>();
        }
    }
}
