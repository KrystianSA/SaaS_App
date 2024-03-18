using SaaS_App.Domain.Common;

namespace SaaS_App.Domain.Entities
{
    public class Account : DomainEntity
    {
        public required string Name { get; set; }
        public DateTimeOffset CreateDate { get; set; }
        public AccountUser AccountUser { get; set; } = new AccountUser();
    }
}
