using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SaaS_App.Domain.Entities;

namespace SaaS_App.Infrastructure.Persistence.Configurations
{
    public class PostConfiguration : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> builder)
        {
            builder.HasOne(e => e.UrlFeed)
                .WithMany(e => e.Posts)
                .HasForeignKey(e => e.UrlFeedId)
                .IsRequired();
        }
    }
}
