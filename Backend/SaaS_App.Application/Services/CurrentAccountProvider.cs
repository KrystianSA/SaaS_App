using EFCoreSecondLevelCacheInterceptor;
using Microsoft.EntityFrameworkCore;
using SaaS_App.Application.Exceptions;
using SaaS_App.Application.Interfaces;
using SaaS_App.Domain.Entities;
using System.Data;

namespace SaaS_App.Application.Services
{
    public class CurrentAccountProvider : ICurrentAccountProvider
    {
        private readonly IAuthenticationDataProvider _authenticationDataProvider;
        private readonly IApplicationDbContext _applicationDbContext;

        public CurrentAccountProvider(IAuthenticationDataProvider authenticationDataProvider,
            IApplicationDbContext applicationDbContext)
        {
            _authenticationDataProvider = authenticationDataProvider;
            _applicationDbContext = applicationDbContext;
        }
        public async Task<Account> GetAuthenticatedAccount()
        {
            var accountId = await GetAccountId();

            if (accountId == null)
            {
                throw new UnauthorizedException();
            }

            var account = await _applicationDbContext.Accounts
                                    .Cacheable()
                                    .FirstOrDefaultAsync(account => account.Id == accountId.Value);

            if (account == null)
            {
                throw new ErrorException("AccountDoesNotExist");
            }

            return account;
        }
        public async Task<int?> GetAccountId()
        {
            var userId = _authenticationDataProvider.GetUserId();
            if (userId != null)
            {
                return await _applicationDbContext.AccountUser
                    .Where(account => account.UserId == userId.Value)
                    .Select(account => (int?)account.AccountId)
                    .Cacheable()
                    .FirstOrDefaultAsync();
            }
            return null;
        }
    }
}
