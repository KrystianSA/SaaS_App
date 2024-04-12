using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SaaS_App.Application.Interfaces;
using SaaS_App.Application.Logic.Abstractions;

namespace SaaS_App.Application.Logic.Account
{
    public static class BlockAccountCommand
    {
        public class Request : IRequest<Result>
        {
            public required string Email { get; set; }
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
                var account = await _dbContext.Accounts.SingleOrDefaultAsync(email => email.Name == request.Email);

                if (account == null)
                {
                    throw new Exception("InvalidLoginOrPassword");
                }

                account.IsActive = false;

                await _dbContext.SaveChangesAsync();

                return new Result() { };
            }
        }
        public class Validator : AbstractValidator<Request>
        { }
    }
}
