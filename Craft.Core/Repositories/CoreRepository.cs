using Craft.Domain.Contracts;
using Craft.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Craft.Core.Repositories;

public class CoreRepository<T, TKey>(DbContext appDbContext, ILogger<CoreRepository<T, TKey>> logger)
    : ICoreRepository<T, TKey> where T : class, IEntity<TKey>, new()
{
    protected readonly DbContext _appDbContext = appDbContext;
    protected readonly DbSet<T> _dbSet = appDbContext.Set<T>();
    protected readonly ILogger<CoreRepository<T, TKey>> _logger = logger;

    public async Task<DbContext> GetDbContextAsync()
        => await Task.FromResult(_appDbContext);

    public async Task<DbSet<T>> GetDbSetAsync()
        => await Task.FromResult(_dbSet);
}

public class CoreRepository<T>(DbContext appDbContext, ILogger<CoreRepository<T, KeyType>> logger)
    : CoreRepository<T, KeyType>(appDbContext, logger), ICoreRepository<T>
        where T : class, IEntity, new()
{ }
