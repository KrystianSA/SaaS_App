using SaaS_App.Domain.Common;

namespace SaaS_App.Domain.Entities
{
    public class Tokens : DomainEntity
    {
        public required string UserId { get; set; }
        public required string Token { get; set; }
        public required DateTime Token_Expiry { get; set; }
    }
}
