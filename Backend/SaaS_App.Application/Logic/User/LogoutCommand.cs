using FluentValidation;
using MediatR;
using SaaS_App.Application.Interfaces;
using SaaS_App.Application.Logic.Abstractions;

namespace SaaS_App.Application.Logic.User
{
    public static class LogoutCommand
    {
        public class Request : IRequest<Result>
        {
        }
        public class Result
        {
        }

        public class Handler : BaseCommandHandler, IRequestHandler<Request, Result>
        {
            public Handler(IApplicationDbContext dbContext,
                ICurrentAccountProvider currentAccountProvider) : base(dbContext, currentAccountProvider)
            { }

            public async Task<Result> Handle(Request request, CancellationToken cancellationToken)
            {
                return new Result() { };
            }
        }
        public class Validator : AbstractValidator<Request>
        { }
    }
}
