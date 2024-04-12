using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SaaS_App.Application.Interfaces;
using SaaS_App.Application.Logic.Abstractions;
using SaaS_App.Application.Logic.User.Helpers;
using SaaS_App.Application.Models.Email;
namespace SaaS_App.Application.Logic.Account
{
    public static class CreateEmailAboutBlockCommand
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
            private const string WEBSITE_NAME = "reset-password";
            private const string SUBJECT_NAME = "Support SaaS_App";
            private readonly IEmailMessageCreator _emailMessageCreator;

            public Handler(IApplicationDbContext dbContext,
                ICurrentAccountProvider currentAccountProvider,
                IEmailMessageCreator emailMessageCreator) : base(dbContext, currentAccountProvider)
            {
                _emailMessageCreator = emailMessageCreator;
            }

            public async Task<Result> Handle(Request request, CancellationToken cancellationToken)
            {
                var parametersForUrl = new Dictionary<string, string>() { };
                var text = "Drogi użytkowniku ! Dostałeś tę wiadomość z powodu zbyt dużej ilości prób zalogowania. Zmień swoje hasło natychmiastowo !";
                var message = _emailMessageCreator.CreateEmailWithUrl(request.Email, SUBJECT_NAME, WEBSITE_NAME, parametersForUrl, text);

                return new Result() { emailData = message }; ;
            }
        }
        public class Validator : AbstractValidator<Request>
        { }
    }
}
