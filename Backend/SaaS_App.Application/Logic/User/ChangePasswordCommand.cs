using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SaaS_App.Application.Interfaces;
using SaaS_App.Application.Logic.Abstractions;

namespace SaaS_App.Application.Logic.User
{
    public static class ChangePasswordCommand
    {
        public class Request : IRequest<Result>
        {
            public required string Token { get; set; }
            public required string NewPassword { get; set; }
        }
        public class Result
        { }

        public class Handler : BaseCommandHandler, IRequestHandler<Request, Result>
        {
            private readonly IPasswordManager _passwordManager;

            public Handler(IApplicationDbContext dbContext,
                ICurrentAccountProvider currentAccountProvider,
                IPasswordManager passwordManager) : base(dbContext, currentAccountProvider)
            {
                _passwordManager = passwordManager;
            }

            public async Task<Result> Handle(Request request, CancellationToken cancellationToken)
            {
                var user = await _dbContext.AccountUser
                        .Include(u => u.User)
                        .Include(t=>t.Token)
                        .Where(token => token.Token.HashedToken == request.Token)
                        .FirstOrDefaultAsync();

                if (user == null)
                {
                    throw new Exception("SomethingWentWrong");
                }

                var newHashedPassword = _passwordManager.HashPassword(request.NewPassword);

                user.User.HashedPassword = newHashedPassword;
                _dbContext.Tokens.Remove(user.Token);
                await _dbContext.SaveChangesAsync();

                return new Result() { };
            }
        }
        public class Validator : AbstractValidator<Request>
        {
            public Validator()
            {
                RuleFor(password => password.NewPassword)
                   .NotEmpty()
                   .MaximumLength(50);
            }
        }
    }
}
