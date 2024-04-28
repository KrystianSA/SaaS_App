using CodeHollow.FeedReader;
using SaaS_App.Application.Interfaces;

namespace SaaS_App.Infrastructure.Rss
{
    public class RssManager : IRssManager
    {
        public async Task<List<string>> ReadUrlsAsync(List<string> urls)
        {
            var posts = new List<string>();
            try
            {
                foreach (var url in urls)
                {
                    var feed = await FeedReader.ReadAsync(url);
                    var result = feed.Items.First();
                    posts.Add(result.Title);
                }
            }
            catch (Exception)
            {

                throw;
            }
            return posts;
        }

        public string GetFeedUrl(string url)
        {
            var feedUrls = FeedReader.GetFeedUrlsFromUrl(url);
            var numberUrls = feedUrls.Count();
            string resultFeed;
            resultFeed = feedUrls.FirstOrDefault().Url;     
            return resultFeed;
        }
    }
}
