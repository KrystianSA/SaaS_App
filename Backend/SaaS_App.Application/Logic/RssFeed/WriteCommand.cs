using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SaaS_App.Application.Exceptions;
using SaaS_App.Application.Interfaces;
using SaaS_App.Application.Logic.Abstractions;
using SaaS_App.Domain.Entities;
using System.Text.RegularExpressions;

namespace SaaS_App.Application.Logic.RssFeed
{
    public static class WriteCommand
    {
        public class Request : IRequest<Result>
        {
            public required string Url { get; set; }
        }
        public class Result
        {
        }

        public class Handler : BaseCommandHandler, IRequestHandler<Request, Result>
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
                var account =  await _currentAccountProvider.GetAuthenticatedAccount();
                
                if(account == null)
                {
                    throw new UnauthorizedException();
                }

                var accountId = account.Id;

                var urlFeed = await _dbContext.UrlFeeds.FirstOrDefaultAsync(url => url.Url == request.Url);

                if(urlFeed != null) 
                {
                    throw new ErrorException("UrlFeedExistInDatabase");
                }

                var url = _rssManager.GetFeedUrl(request.Url);

                if(url == null)
                {
                    throw new ErrorException("UnfortunatelyFeedUrlsDoesNotExit");
                }

                var newUrlFeed = new UrlFeed()
                {
                    AccountId = accountId,
                    Url = url,
                };

                _dbContext.UrlFeeds.Add(newUrlFeed);
                await _dbContext.SaveChangesAsync();

                return new Result() { };
            }
        }
        public class Validator : AbstractValidator<Request>
        {
            public Validator()
            {
                RuleFor(x => x.Url)
                    .Must(ValidateUrlWithRegex)
                    .WithMessage("InvalidAdressUrl");
            }

            private bool ValidateUrlWithRegex(string url)
            {
                var urlRegex = new Regex(
                    @"^(https?|ftps?):\/\/(?:[a-zA-Z0-9]" +
                    @"(?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?\.)+[a-zA-Z]{2,}" +
                    @"(?::(?:0|[1-9]\d{0,3}|[1-5]\d{4}|6[0-4]\d{3}" +
                    @"|65[0-4]\d{2}|655[0-2]\d|6553[0-5]))?" +
                    @"(?:\/(?:[-a-zA-Z0-9@%_\+.~#?&=]+\/?)*)?$",
                    RegexOptions.IgnoreCase);

                urlRegex.Matches(url);

                return urlRegex.IsMatch(url);
            }
        }
    }
}
