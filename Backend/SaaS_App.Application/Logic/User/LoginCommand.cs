using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SaaS_App.Application.Exceptions;
using SaaS_App.Application.Interfaces;
using SaaS_App.Application.Logic.Abstractions;

namespace SaaS_App.Application.Logic.User
{
    public static class LoginCommand
    {
        public class Request : IRequest<Result>
        {
            public required string Email { get; set; }
            public required string Password { get; set; }
        }
        public class Result
        {
            public required int UserId { get; set; }
        }

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
                var userData = await _dbContext.AccountUser
                    .Include(user => user.User)
                    .Include(account => account.Account)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(au => au.User.Email == request.Email || au.Account.Name == request.Email);

                if (userData != null && userData.Account.IsActive)
                {
                    if (_passwordManager.VerifyPassword(userData.User.HashedPassword, request.Password))
                    {
                        return new Result() { UserId = userData.UserId };
                    }
                }
                throw new ErrorException("InvalidLoginOrPassword");
            }
        }
        public class Validator : AbstractValidator<Request>
        {
            public Validator()
            {
                RuleFor(email => email.Email)
                    .NotEmpty()
                    .EmailAddress();


                RuleFor(password => password.Password)
                   .NotEmpty();
            }
        }
    }
}
