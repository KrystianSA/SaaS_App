using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SaaS_App.Application.Exceptions;
using SaaS_App.Application.Interfaces;
using SaaS_App.Application.Logic.Abstractions;

namespace SaaS_App.Application.Logic.RssFeed
{
    public static class ReadCommand
    {
        public class Request : IRequest<Result>
        {
            public required string Url { get; set; }
        }
        public class Result
        {
            public required string Message { get; set; }
        }

        public class Handler : BaseQueryHandler, IRequestHandler<Request, Result>
        {
            private readonly IRssManager _rssManager;

            public Handler(IApplicationDbContext dbContext,
                ICurrentAccountProvider currentAccountProvider,
                IRssManager rssManager) : base(dbContext, currentAccountProvider)
            {
                _rssManager = rssManager;
            }

            public async Task<Result> Handle(Request request, CancellationToken cancellationToken)
            {
                var result = await _rssManager.ReadAsync(request.Url);
                return new Result() { Message = result };
            }
        }
        public class Validator : AbstractValidator<Request>
        {
            public Validator()
            {
                
            }
        }
    }
}
