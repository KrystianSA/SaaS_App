using SaaS_App.Domain.Common;

namespace SaaS_App.Domain.Entities
{
    public class AccountUser : DomainEntity
    {
        public int UserId { get; set; }
        public User User { get; set; } = default;
        public int AccountId { get; set; }
        public Account Account { get; set; } = default;
        public Tokens Token { get; set; } = null!;
    }
}
