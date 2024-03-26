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
            public required string Email { get; set; }
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
                var user = await _dbContext.Users.SingleOrDefaultAsync(email => email.Email == request.Email);

                if (user == null)
                {
                    throw new ErrorException("InvalidAdressEmail");
                }

                var isOldPassword = _passwordManager.VerifyPassword(user.HashedPassword, request.NewPassword);
                
                if (isOldPassword)
                {
                    throw new ErrorException("SomethingWentWrong");
                }

                var newHashedPassword = _passwordManager.HashPassword(request.NewPassword);
                var updatedPassword = await _dbContext.Users.ExecuteUpdateAsync(property => property
                                            .SetProperty(password => password.HashedPassword, newHashedPassword));

                await _dbContext.SaveChangesAsync();

                return new Result() { };
            }
        }
        public class Validator : AbstractValidator<Request>
        {
            public Validator()
            {
                RuleFor(email => email.Email)
                    .NotEmpty()
                    .EmailAddress();


                RuleFor(password => password.NewPassword)
                   .NotEmpty()
                   .MaximumLength(50);
            }
        }
    }
}
