using SaaS_App.Domain.Common;

namespace SaaS_App.Domain.Entities
{
    public class Post : DomainEntity
    {
        public required int UrlFeedId { get; set; }
        public UrlFeed UrlFeed { get; set; } = null!;
        public required string Title { get; set; }
        public required string Url { get; set; }
    }
}
