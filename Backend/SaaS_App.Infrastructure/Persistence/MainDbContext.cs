using Microsoft.EntityFrameworkCore;
using SaaS_App.Application.Interfaces;
using SaaS_App.Domain.Entities;

namespace SaaS_App.Infrastructure.Persistence
{
    public class MainDbContext : DbContext, IApplicationDbContext 
    {
        public MainDbContext(DbContextOptions<MainDbContext> options) : base(options)
        {

        }
        public DbSet<User> Users { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<AccountUser> AccountUser { get; set; }
        public DbSet<Tokens> Tokens { get; set; }
        public DbSet<UrlFeed> UrlFeeds { get; set; }
        public DbSet<Post> Posts { get; set; }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Properties<decimal>().HavePrecision(18, 4);

            base.ConfigureConventions(configurationBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(MainDbContext).Assembly);
        }
    }
}
