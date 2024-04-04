using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SaaS_App.Application.Exceptions;
using SaaS_App.Application.Interfaces;
using SaaS_App.Application.Logic.Abstractions;
using SaaS_App.Application.Logic.User.Helpers;
using SaaS_App.Application.Models.Email;

namespace SaaS_App.Application.Logic.User
{
    public static class CreateEmailToRestPasswordCommand
    {
        public class Request : IRequest<Result>
        {
            public required string Email { get; set; }
        }
        public class Result
        {
            public required EmailMessage emailData { get; set; }
        }

        public class Handler : BaseCommandHandler, IRequestHandler<Request, Result>
        {
            private const string SUBJECT_NAME = "Link to reset your password";
            private const string SITE_NAME = "reset-password";
            private readonly IConfiguration _configuration;

            public Handler(IApplicationDbContext dbContext,
                ICurrentAccountProvider currentAccountProvider,
                IConfiguration configuration) : base(dbContext, currentAccountProvider)
            {
                _configuration = configuration;
            }

            public async Task<Result> Handle(Request request, CancellationToken cancellationToken)
            {
                var user = await _dbContext.AccountUser
                                    .Include(u => u.User)
                                    .Include(t => t.Token)
                                    .Where(email => email.User.Email == request.Email)
                                    .FirstOrDefaultAsync();

                if (user == null)
                {
                    throw new ErrorException("EmailHasBeenSuccesfullySent");
                }

                var message = new EmailMessage()
                {
                    To = request.Email,
                    Subject = SUBJECT_NAME,
                    Content = ""
                };
                var domainName = _configuration["WebAppBaseUrl"];
                var siteName = SITE_NAME;
                var hashedToken = user.Token.HashedToken;
                var parameters = new Dictionary<string, string>()
                {
                    {"token", hashedToken }
                };
                message.Content = UrlGenerator.GenerateUrl(domainName!, siteName, parameters);

                return new Result() { emailData = message }; ;
            }
        }
        public class Validator : AbstractValidator<Request>
        {
            public Validator()
            {
                RuleFor(email => email.Email)
                    .NotEmpty()
                    .EmailAddress();
            }
        }
    }
}
