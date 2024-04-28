using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SaaS_App.Domain.Entities;

namespace SaaS_App.Infrastructure.Persistence.Configurations
{
    public class UrlFeedConfiguration : IEntityTypeConfiguration<UrlFeed>
    {
        public void Configure(EntityTypeBuilder<UrlFeed> builder)
        {
            builder.HasOne(e => e.Account)
                .WithMany(e => e.UrlFeeds)
                .HasForeignKey(b => b.AccountId)
                .IsRequired();
        }
    }
}
