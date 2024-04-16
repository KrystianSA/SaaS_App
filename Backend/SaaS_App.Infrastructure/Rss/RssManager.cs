using CodeHollow.FeedReader;
using SaaS_App.Application.Interfaces;

namespace SaaS_App.Infrastructure.Rss
{
    public class RssManager : IRssManager
    {
        public async Task<string> ReadAsync(string url)
        {
            var urls = FeedReader.GetFeedUrlsFromUrl(url);

            string feedUrl = "";
            if (urls.Count() < 1) // no url - probably the url is already the right feed url
                feedUrl = url;
            else if (urls.Count() == 1)
                feedUrl = urls.First().Url;
            else if (urls.Count() == 2) // if 2 urls, then its usually a feed and a comments feed, so take the first per default
                feedUrl = urls.First().Url;
            else
            {
                // show all urls and let the user select (or take the first or ...)
                // ...
                feedUrl = urls.First().Url;
            }
            var feed = await FeedReader.ReadAsync(feedUrl);
            var result = feed.Items.FirstOrDefault();
            if (result.PublishingDate < DateTime.UtcNow)
            {
                return result.Link;
            }
            return "Brak nowych postów";
        }
    }
}
