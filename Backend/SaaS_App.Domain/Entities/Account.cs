using SaaS_App.Domain.Common;

namespace SaaS_App.Domain.Entities
{
    public class Account : DomainEntity
    {
        public required string Name { get; set; }
        public DateTimeOffset CreateDate { get; set; }
        public bool IsActive { get; set; } = false;
        public AccountUser AccountUser { get; set; } = new AccountUser();
        public ICollection<UrlFeed> UrlFeeds { get; set; } = new List<UrlFeed>();
    }
}
