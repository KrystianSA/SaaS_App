using SaaS_App.Domain.Common;

namespace SaaS_App.Domain.Entities
{
    public class UrlFeed : DomainEntity
    {
        public required int AccountId { get; set; }
        public Account Account { get; set; } = null!;
        public required string Url { get; set; }
        public ICollection<Post> Posts { get; set; } = new List<Post>();
    }
}
