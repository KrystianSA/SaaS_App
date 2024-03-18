using SaaS_App.Domain.Entities;

namespace SaaS_App.Application.Interfaces
{
    public interface ICurrentAccountProvider
    {
        Task<Account> GetAuthenticatedAccount();
        Task<int?> GetAccountId();
    }
}
