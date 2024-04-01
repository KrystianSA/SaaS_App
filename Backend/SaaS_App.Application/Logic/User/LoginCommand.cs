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
                var user = await _dbContext.Users.FirstOrDefaultAsync(user => user.Email == request.Email);
                var account = await _dbContext.Accounts.FirstOrDefaultAsync(account => account.Name == request.Email);

                if (user != null && account!.IsActive == true)
                {
                    if (_passwordManager.VerifyPassword(user.HashedPassword, request.Password))
                    {
                        return new Result() { UserId = user.Id };
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
