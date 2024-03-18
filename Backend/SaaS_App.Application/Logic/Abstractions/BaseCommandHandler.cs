using SaaS_App.Application.Interfaces;

namespace SaaS_App.Application.Logic.Abstractions
{
    public abstract class BaseCommandHandler
    {
        protected readonly IApplicationDbContext _dbContext;
        protected readonly ICurrentAccountProvider _currentAccountProvider;

        public BaseCommandHandler(IApplicationDbContext dbContext,
            ICurrentAccountProvider currentAccountProvider)
        {
            _dbContext = dbContext;
            _currentAccountProvider = currentAccountProvider;
        }
    }
}
