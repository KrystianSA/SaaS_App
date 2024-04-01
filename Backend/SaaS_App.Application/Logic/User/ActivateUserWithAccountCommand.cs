using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SaaS_App.Application.Interfaces;
using SaaS_App.Application.Logic.Abstractions;

namespace SaaS_App.Application.Logic.User
{
    public static class ActivateUserWithAccountCommand
    {
        public class Request : IRequest<Result>
        {
            public required string Email { get; set; }
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
                var token = await _dbContext.Tokens.FirstOrDefaultAsync(token => token.Token == request.Token);
                if (token == null)
                {
                    throw new Exception("SomethingWentWrong");
                }
                await _dbContext.Accounts.Where(account => account.Name == request.Email).
                                                ExecuteUpdateAsync(account => account.SetProperty(
                                                    active => active.IsActive, true));
                await _dbContext.Tokens.Where(token => token.Token_Expiry < DateTime.UtcNow
                                                || token.Token == request.Token).ExecuteDeleteAsync();
                return new Result() { };
            }
        }
        public class Validator : AbstractValidator<Request>
        { }
    }
}
