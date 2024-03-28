using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SaaS_App.Application.Exceptions;
using SaaS_App.Application.Helpers;
using SaaS_App.Application.Interfaces;
using SaaS_App.Application.Logic.Abstractions;
using SaaS_App.Domain.Entities;
using SaaS_App.Infrastructure.Email;

namespace SaaS_App.Application.Logic.User
{
    public static class SendRestPasswordLinkCommand
    {
        public class Request : IRequest<Result>
        {
            public required string Email { get; set; }
            public required string Subject { get; set; }
        }
        public class Result
        {
            public required string ResetLink { get; set; }
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
                var user = await _dbContext.Users.SingleOrDefaultAsync(email => email.Email == request.Email);

                if (user == null)
                {
                    throw new ErrorException("EmailHasBeenSuccesfullySent");
                }

                var token = TokenUtils.GenerateToken(LENGTH_TOKEN);
                var hashedToken = TokenUtils.GenerateHash(token);


                var tokenModel = new Tokens()
                {
                    UserId = user.Email,
                    Token = hashedToken,
                    Token_Expiry = DateTime.UtcNow.AddDays(1),
                };

                await _dbContext.Tokens.AddAsync(tokenModel);
                await _dbContext.SaveChangesAsync();

                return new Result() { ResetLink = "http://localhost:5164/reset-password?token=" + hashedToken };
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
