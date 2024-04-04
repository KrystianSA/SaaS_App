using SaaS_App.Domain.Common;

namespace SaaS_App.Domain.Entities
{
    public class Tokens : DomainEntity
    {
        public int RecipientId { get; set; }
        public AccountUser AccountUser { get; set; } = null!;
        public required string HashedToken { get; set; }
        public required DateTime Token_Expiry { get; set; }
    }
}
