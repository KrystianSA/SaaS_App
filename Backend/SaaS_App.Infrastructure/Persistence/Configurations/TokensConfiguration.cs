using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SaaS_App.Domain.Entities;

namespace SaaS_App.Infrastructure.Persistence.Configurations
{
    public class TokensConfiguration : IEntityTypeConfiguration<Tokens>
    {
        public void Configure(EntityTypeBuilder<Tokens> builder)
        {
            builder.HasOne(p => p.AccountUser)
                .WithOne(b => b.Token)
                .HasForeignKey<Tokens>(b => b.RecipientId);
        }
    }
}
