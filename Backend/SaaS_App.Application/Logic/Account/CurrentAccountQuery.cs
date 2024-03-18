using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SaaS_App.Application.Exceptions;
using SaaS_App.Application.Interfaces;
using SaaS_App.Application.Logic.Abstractions;

namespace SaaS_App.Application.Logic.Account
{
    public static class CurrentAccountQuery
    {
        public class Request : IRequest<Result>
        {
        }
        public class Result
        {
            public required string Email { get; set; }
        }

        public class Handler : BaseQueryHandler, IRequestHandler<Request, Result>
        {
            public Handler(IApplicationDbContext dbContext,
                ICurrentAccountProvider currentAccountProvider,
                IAuthenticationDataProvider authenticationDataProvider) : base(dbContext, currentAccountProvider)
            {
            }

            public async Task<Result> Handle(Request request, CancellationToken cancellationToken)
            {
                var account = await _currentAccountProvider.GetAuthenticatedAccount();
                return new Result() { Email = account.Name };
            }
        }
        public class Validator : AbstractValidator<Request>
        { }
    }
}
