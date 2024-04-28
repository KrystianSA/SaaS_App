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
    public static class ReadQuery
    {
        public class Request : IRequest<Result>
        {
        }
        public class Result
        {
            public required List<string> Posts { get; set; }
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
                var account = await _currentAccountProvider.GetAuthenticatedAccount();

                if (account == null)
                {
                    throw new UnauthorizedException();
                }

                var accountId = account.Id;
                var urlFeedObjects = _dbContext.UrlFeeds.Where(id => id.AccountId == accountId).ToList();
                var urlsFeed = urlFeedObjects.Select(url => url.Url).ToList();
                var posts = await _rssManager.ReadUrlsAsync(urlsFeed);

                var newPosts = new List<string>();

                foreach (var post in posts)
                {
                    var postFromDb = _dbContext.Posts.FirstOrDefault(title => title.Title == post);
                    if (postFromDb == null)
                    {
                        newPosts.Add(post);
                    }
                }
                
                return new Result() { Posts = newPosts };
            }
        }
        public class Validator : AbstractValidator<Request>
        {

        }
    }
}
