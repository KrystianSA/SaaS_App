using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SaaS_App.Application.Interfaces;
using SaaS_App.Application.Logic.Abstractions;

namespace SaaS_App.Application.Logic.Account
{
    public static class ActivateAccountCommand
    {
        public class Request : IRequest<Result>
        {
            public required string Token { get; set; }
        }
        public class Result
        {
        }

        public class Handler : BaseCommandHandler, IRequestHandler<Request, Result>
        {
            public Handler(IApplicationDbContext dbContext,
                ICurrentAccountProvider currentAccountProvider) : base(dbContext, currentAccountProvider)
            { }

            public async Task<Result> Handle(Request request, CancellationToken cancellationToken)
            {
                var account = await _dbContext.Tokens
                    .Include(au => au.AccountUser.Account)
                    .Where(token => token.HashedToken == request.Token)
                    .FirstOrDefaultAsync();

                if (account == null)
                {
                    throw new Exception("SomethingWentWrong");
                }

                account.AccountUser.Account.IsActive = true;

                _dbContext.Tokens.Remove(account);

                await _dbContext.SaveChangesAsync();

                return new Result() { };
            }
        }
        public class Validator : AbstractValidator<Request>
        { }
    }
}
