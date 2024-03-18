using SaaS_App.Domain.Common;

namespace SaaS_App.Domain.Entities
{
    public class User : DomainEntity
    {
        public required string Email { get; set; }
        public required string HashedPassword { get; set; }
        public DateTimeOffset RegisterDate { get; set; }
        public AccountUser AccountUser { get; set; } = new AccountUser();
    }
}