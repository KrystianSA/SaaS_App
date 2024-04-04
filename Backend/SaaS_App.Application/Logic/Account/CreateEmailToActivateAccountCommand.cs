using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SaaS_App.Application.Interfaces;
using SaaS_App.Application.Logic.Abstractions;
using SaaS_App.Application.Logic.User.Helpers;
using SaaS_App.Application.Models.Email;

namespace SaaS_App.Application.Logic.Account
{
    public static class CreateEmailToActivateAccountCommand
    {
        public class Request : IRequest<Result>
        {
            public required int AccountId { get; set; }
        }
        public class Result
        {
            public required EmailMessage emailData { get; set; }
        }

        public class Handler : BaseCommandHandler, IRequestHandler<Request, Result>
        {
            private readonly IConfiguration _configuration;
            private const string SITE_NAME = "activate-account";
            private const string SUBJECT_NAME = "Link to activate account";

            public Handler(IApplicationDbContext dbContext,
                ICurrentAccountProvider currentAccountProvider,
                IConfiguration configuration) : base(dbContext, currentAccountProvider)
            {
                _configuration = configuration;
            }

            public async Task<Result> Handle(Request request, CancellationToken cancellationToken)
            {
                var account = await _dbContext.Tokens
                        .Include(a => a.AccountUser.Account)
                        .Where(accountId => accountId.AccountUser.Account.Id == request.AccountId)
                        .FirstOrDefaultAsync();

                if (account == null)
                {
                    throw new Exception("SomethingWentWrong");
                }

                var email = account.AccountUser.Account.Name;
                var message = new EmailMessage()
                {
                    To = email,
                    Subject = SUBJECT_NAME,
                    Content = ""
                };
                var domainName = _configuration["WebAppBaseUrl"];
                var siteName = SITE_NAME;
                var hashedToken = account.HashedToken;
                var parameters = new Dictionary<string, string>()
                {
                    {"token", hashedToken }
                };
                message.Content = UrlGenerator.GenerateUrl(domainName!, siteName, parameters);

                return new Result() { emailData = message };
            }
        }
        public class Validator : AbstractValidator<Request>
        { }
    }
}
