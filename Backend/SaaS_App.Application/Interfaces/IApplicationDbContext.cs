﻿using Microsoft.EntityFrameworkCore;
using SaaS_App.Domain.Entities;

namespace SaaS_App.Application.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<User> Users { get; set; }
        DbSet<Account> Accounts { get; set; }
        DbSet<AccountUser> AccountUser { get; set; }
        DbSet<Tokens> Tokens { get; set; }
        DbSet<UrlFeed> UrlFeeds { get; set; }
        DbSet<Post> Posts { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
