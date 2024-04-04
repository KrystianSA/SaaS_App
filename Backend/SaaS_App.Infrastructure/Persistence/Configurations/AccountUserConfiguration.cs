using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SaaS_App.Domain.Entities;

namespace SaaS_App.Infrastructure.Persistence.Configurations
{
    public class AccountUserConfiguration : IEntityTypeConfiguration<AccountUser>
    {
        public void Configure(EntityTypeBuilder<AccountUser> builder)
        {
            builder.HasOne(p => p.Account)
                .WithOne(b => b.AccountUser)
                .HasForeignKey<AccountUser>(b => b.AccountId);

            builder.HasOne(p => p.User)
                .WithOne(b => b.AccountUser)
                .HasForeignKey<AccountUser>(b => b.UserId);
        }
    }
}
