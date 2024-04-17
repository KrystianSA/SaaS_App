using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SaaS_App.Application.Exceptions;
using SaaS_App.Application.Interfaces;
using SaaS_App.Application.Logic.Abstractions;
using System.Text.RegularExpressions;

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
                var result = await _rssManager.ReadNewFeedAsync(request.Url);
                return new Result() { Message = result };
            }
        }
        public class Validator : AbstractValidator<Request>
        {
            public Validator()
            {
                RuleFor(x => x.Url)
                    .Must(ValidateUrlWithRegex)
                    .WithMessage("InvalidAdressEmail");
            }

            private bool ValidateUrlWithRegex(string url)
            {
                var urlRegex = new Regex(
                    @"^(https?):\/\/(?:[a-zA-Z0-9]" +
                            @"(?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?\.)+[a-zA-Z]{2,}" +
                            @"(?::(?:0|[1-9]\d{0,3}|[1-5]\d{4}|6[0-4]\d{3}" +
                            @"(?:\/(?:[-a-zA-Z0-9@%_\+.~#?&=]+\/?)*)?$",
                    RegexOptions.IgnoreCase);

                urlRegex.Matches(url);

                return urlRegex.IsMatch(url);
            }
        }
    }
}
