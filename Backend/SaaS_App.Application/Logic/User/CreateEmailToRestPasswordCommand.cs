using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SaaS_App.Application.Exceptions;
using SaaS_App.Application.Interfaces;
using SaaS_App.Application.Logic.Abstractions;
using SaaS_App.Application.Logic.User.Helpers;
using SaaS_App.Application.Models.Email;
using SaaS_App.Domain.Entities;

namespace SaaS_App.Application.Logic.User
{
    public static class CreateEmailToRestPasswordCommand
    {
        public class Request : IRequest<Result>
        {
            public required string Email { get; set; }
        }
        public class Result
        {
            public required EmailMessage emailData { get; set; }
        }

        public class Handler : BaseCommandHandler, IRequestHandler<Request, Result>
        {
            private readonly IEmailMessageCreator _emailMessageCreator;
            private const string SUBJECT_NAME = "Link to reset your password";
            private const string WEBSITE_NAME = "reset-password";

            public Handler(IApplicationDbContext dbContext,
                ICurrentAccountProvider currentAccountProvider,
                IEmailMessageCreator emailMessageCreator) : base(dbContext, currentAccountProvider)
            {
                _emailMessageCreator = emailMessageCreator;
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

                var parameters = new Dictionary<string, string>()
                {
                    { "token", user.Token.HashedToken}
                };

                var message = _emailMessageCreator.CreateEmailWithUrl(user.User.Email, SUBJECT_NAME, WEBSITE_NAME, parameters);             

                return new Result() { emailData = message }; ;
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
