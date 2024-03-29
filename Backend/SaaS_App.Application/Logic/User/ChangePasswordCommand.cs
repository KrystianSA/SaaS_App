using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SaaS_App.Application.Exceptions;
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
                var token = await _dbContext.Tokens.FirstOrDefaultAsync(token=> token.Token == request.Token);

                if (token == null) 
                {
                    throw new Exception("SomethingWentWrong");
                }
           
                var isTokenValid = DateTime.UtcNow > token.Token_Expiry;
                if (isTokenValid) { throw new ErrorException("TokenIsExpired"); }

                var newHashedPassword = _passwordManager.HashPassword(request.NewPassword);

                await _dbContext.Users.ExecuteUpdateAsync(property => property
                                            .SetProperty(password => password.HashedPassword, newHashedPassword));
                await _dbContext.Tokens.Where(token => token.Token_Expiry < DateTime.UtcNow
                                                || token.Token == request.Token).ExecuteDeleteAsync();
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
