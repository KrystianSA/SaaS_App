using CodeHollow.FeedReader;
using SaaS_App.Application.Interfaces;

namespace SaaS_App.Infrastructure.Rss
{
    public class RssManager : IRssManager
    {
        public async Task<string> ReadNewFeedAsync(string url)
        {
            string feedUrl = GetFeedUrl(url);
            var feed = await FeedReader.ReadAsync(feedUrl);
            var result = feed.Items.First();
            return result.Link;
        }

        private string GetFeedUrl(string url)
        {
            var feedUrls = FeedReader.GetFeedUrlsFromUrl(url);
            var numberUrls = feedUrls.Count();
            string resultFeed;

            try
            {
                resultFeed = feedUrls.First().Url;
            }
            catch (Exception)
            {
                throw new Exception("Brak odpowiedniego Url");
            }
              
            return resultFeed;
        }
    }
}
