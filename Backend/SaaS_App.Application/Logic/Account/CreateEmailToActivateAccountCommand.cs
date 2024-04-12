using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SaaS_App.Application.Interfaces;
using SaaS_App.Application.Logic.Abstractions;
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
            private const string WEBSITE_NAME = "activate-account";
            private const string SUBJECT_NAME = "Link to activate account";
            private readonly IEmailMessageCreator _emailMessageCreator;

            public Handler(IApplicationDbContext dbContext,
                ICurrentAccountProvider currentAccountProvider,
                IEmailMessageCreator emailMessageCreator) : base(dbContext, currentAccountProvider)
            {
                _emailMessageCreator = emailMessageCreator;
            }

            public async Task<Result> Handle(Request request, CancellationToken cancellationToken)
            {
                var account = await _dbContext.AccountUser
                        .Include(a => a.Token)
                        .Include(a => a.Account)
                        .Where(accountId => accountId.AccountId == request.AccountId)
                        .FirstOrDefaultAsync();

                if (account == null)
                {
                    throw new Exception("SomethingWentWrong");
                }

                var parameters = new Dictionary<string, string>()
                {
                    { "token", account.Token.HashedToken}
                };
                var message = _emailMessageCreator.CreateEmailWithUrl(account.Account.Name, SUBJECT_NAME, WEBSITE_NAME, parameters);

                return new Result() { emailData = message }; ;
            }
        }
        public class Validator : AbstractValidator<Request>
        { }
    }
}
