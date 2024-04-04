using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SaaS_App.Application.Exceptions;
using SaaS_App.Application.Interfaces;
using SaaS_App.Application.Logic.Abstractions;
using SaaS_App.Application.Logic.User.Helpers;
using SaaS_App.Domain.Entities;

namespace SaaS_App.Application.Logic.User
{
    public static class CreateRequirementsToResetPasswordCommand
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
            public const int LENGTH_TOKEN = 128;

            public Handler(IApplicationDbContext dbContext,
                ICurrentAccountProvider currentAccountProvider) : base(dbContext, currentAccountProvider)
            {
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

                var hashedToken = TokenUtils.GenerateToken(LENGTH_TOKEN).GenerateHash();

                var tokenModel = new Tokens()
                {
                    AccountUser = user,
                    HashedToken = hashedToken,
                    Token_Expiry = DateTime.UtcNow.AddDays(1),
                };

                await _dbContext.Tokens.AddAsync(tokenModel);
                await _dbContext.SaveChangesAsync();

                return new Result(){ };
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
