using EFCoreSecondLevelCacheInterceptor;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SaaS_App.Application.Exceptions;
using SaaS_App.Application.Interfaces;
using SaaS_App.Application.Logic.Abstractions;

namespace SaaS_App.Application.Logic.User
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
            private readonly IAuthenticationDataProvider _authenticationDataProvider;

            public Handler(IApplicationDbContext dbContext,
                ICurrentAccountProvider currentAccountProvider,
                IAuthenticationDataProvider authenticationDataProvider) : base(dbContext, currentAccountProvider)
            {
                _authenticationDataProvider = authenticationDataProvider;
            }

            public async Task<Result> Handle(Request request, CancellationToken cancellationToken)
            {
                var userId = _authenticationDataProvider.GetUserId();

                if (userId.HasValue)
                {
                    var user = await _dbContext.Users.Cacheable().FirstOrDefaultAsync(user => user.Id == userId);
                    if (user != null)
                    {
                        return new Result() { Email = user.Email };
                    }
                }

                throw new UnauthorizedException();
            }
        }
        public class Validator : AbstractValidator<Request>
        { }
    }
}
